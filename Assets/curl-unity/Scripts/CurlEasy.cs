using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurlUnity
{
    public class CurlException : Exception
    {
        public string reason;
        public CURLE error;

        public CurlException(CURLE error, string reason) : base($"{reason}: {error}")
        {
            this.error = error;
            this.reason = reason;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CurlBlob
    {
        public IntPtr data;
        public UIntPtr len;
        public uint flags;
    }

    public class CurlEasy : IDisposable
    {
        public delegate void PerformCallback(CURLE result, CurlEasy easy);
        public delegate void ProgressCallback(long dltotal, long dlnow, long ultotal, long ulnow, CurlEasy easy);

        public Uri uri { get; set; }
        public string method { get; set; } = "GET";
        public string contentType { get; set; } = "application/text";
        public string outputPath { get; set; }
        public long rangeStart { get; set; } = 0;
        public long rangeEnd { get; set; } = 0;
        public int timeout { get; set; } = 0;
        public int connectionTimeout { get; set; } = 5000;
        public int maxRetryCount { get; set; } = 5;
        public int retryInterval { get; set; } = 1000;
        public int lowSpeedLimit { get; set; } = 100;
        public int lowSpeedTimeout { get; set; } = 30;
        public int outSpeedLimit { get; set; } = 0;
        public int inSpeedLimit { get; set; } = 0;
        public bool insecure { get; set; }
        public bool disableExpect { get; set; } = true;
        public bool followRedirect { get; set; } = true;
        public byte[] outData { get; set; }
        public byte[] inData { get; private set; }
        public string httpVersion { get; private set; }
        public int status { get; private set; }
        public string message { get; private set; }
        public bool running { get; private set; }
        public PerformCallback performCallback { get; set; }
        public ProgressCallback progressCallback { get; set; }
        public bool debug { get; set; }
        public CurlDecoder decoder { get; set; }
        public Exception exception { get; set; }

        public string outText
        {
            get
            {
                return outData != null ? Encoding.UTF8.GetString(outData) : null;
            }
            set
            {
                outData = value != null ? Encoding.UTF8.GetBytes(value) : null;
            }
        }

        public string inText
        {
            get
            {
                return inData != null ? Encoding.UTF8.GetString(inData) : null;
            }
            set
            {
                inData = value != null ? Encoding.UTF8.GetBytes(value) : null;
            }
        }

        public long outDataLength
        {
            get
            {
                GetInfo(CURLINFO.CONTENT_LENGTH_UPLOAD, out double uploadLength);
                return (long)uploadLength;
            }
        }

        public long inDataLength
        {
            get
            {
                GetInfo(CURLINFO.CONTENT_LENGTH_DOWNLOAD, out double downloadLength);
                return (long)downloadLength;
            }
        }

        public long recievedDataLength { get; private set; }

        private IntPtr easyPtr;
        private CurlMulti multi;
        private int retryCount;

        public class CaseInsensiveComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.Equals(y, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return obj.ToLower().GetHashCode();
            }
        }

        [Serializable]
        public class HeaderDict : Dictionary<string, string>
        {
            public HeaderDict()
                : base(new CaseInsensiveComparer())
            { }
        }

        public HeaderDict userHeader { get; private set; }
        public HeaderDict outHeader { get; set; }
        public HeaderDict inHeader { get; private set; }

        private Stream responseHeaderStream;
        private Stream responseBodyStream;

        private GCHandle thisHandle;

#if !ALLOW_UNSAFE
        private byte[] buffer;
        private byte[] AcquireBuffer(long size)
        {
            if (buffer == null || buffer.Length < size)
            {
                buffer = new byte[size];
            }
            return buffer;
        }
#endif

        static readonly CurlBlob cacert;

        static CurlEasy()
        {
            var res = Lib.curl_global_init((long)CURLGLOBAL.ALL);
            if (res != CURLE.OK)
            {
                throw new CurlException(res, "curl_global_init");
            }
            var CACERT = CurlCA.CACERT;
            var cacertptr = Marshal.AllocHGlobal(CACERT.Length);
            Marshal.Copy(CACERT, 0, cacertptr, CACERT.Length);

            cacert = new CurlBlob
            {
                data = cacertptr,
                len = (UIntPtr)CACERT.Length,
                flags = 0,
            };
        }

        public static string Version()
        {
            var ptr = Lib.curl_version();
            return Marshal.PtrToStringUTF8(ptr);
        }

        public CurlEasy(IntPtr ptr = default)
        {
            if (ptr != IntPtr.Zero)
            {
                easyPtr = ptr;
            }
            else
            {
                easyPtr = Lib.curl_easy_init();
            }

            if (easyPtr == IntPtr.Zero)
            {
                throw new CurlException(CURLE.OK, "curl_easy_init");
            }
        }

        internal void CleanUp()
        {
            if (easyPtr != IntPtr.Zero)
            {
                Lib.curl_easy_cleanup(easyPtr);
                easyPtr = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Abort();
        }

        public void Reset()
        {
            Lib.curl_easy_reset(easyPtr);
        }

        public CurlEasy Duplicate()
        {
            return new CurlEasy(Lib.curl_easy_duphandle(easyPtr));
        }

        #region SetOpt
        private static CURLE CK(CURLE code, CURLOPT options, object value)
        {
            if (code != CURLE.OK)
            {
                throw new CurlException(code, $"curl_easy_setopt({options}, {value})");
            }
            return code;
        }

        public CURLE SetOpt(CURLOPT options, long value)
        {
            return CK(Lib.curl_easy_setopt_int(easyPtr, options, value), options, value);
        }

        public CURLE SetOpt(CURLOPT options, bool value)
        {
            return CK(Lib.curl_easy_setopt_int(easyPtr, options, value ? 1 : 0), options, value);
        }

        public CURLE SetOpt(CURLOPT options, string value)
        {
            return CK(Lib.curl_easy_setopt_str(easyPtr, options, value), options, value);
        }

        public CURLE SetOpt(CURLOPT options, IntPtr value)
        {
            return CK(Lib.curl_easy_setopt_ptr(easyPtr, options, value), options, value);
        }

        public CURLE SetOpt(CURLOPT options, byte[] value)
        {
            return CK(Lib.curl_easy_setopt_ptr(easyPtr, options, value), options, value);
        }

        public CURLE SetOpt(CURLOPT options, CurlBlob blob)
        {
            return CK(Lib.curl_easy_setopt_ptr(easyPtr, options, ref blob), options, blob);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.WriteFunction value)
        {
            return CK(Lib.curl_easy_setopt_ptr(easyPtr, options, value), options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.HeaderFunction value)
        {
            return CK(Lib.curl_easy_setopt_ptr(easyPtr, options, value), options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.DebugFunction value)
        {
            return CK(Lib.curl_easy_setopt_ptr(easyPtr, options, value), options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.ProgressFunction value)
        {
            return CK(Lib.curl_easy_setopt_ptr(easyPtr, options, value), options, value);
        }

        #endregion
        #region GetInfo
        public CURLE GetInfo(CURLINFO info, out long value)
        {
            value = 0;
            return Lib.curl_easy_getinfo_ptr(easyPtr, info, ref value);
        }

        public CURLE GetInfo(CURLINFO info, out double value)
        {
            value = 0;
            return Lib.curl_easy_getinfo_ptr(easyPtr, info, ref value);
        }

        public CURLE GetInfo(CURLINFO info, out string value)
        {
            value = null;
            IntPtr ptr = IntPtr.Zero;
            var result = Lib.curl_easy_getinfo_ptr(easyPtr, info, ref ptr);
            if (ptr != IntPtr.Zero)
            {
                value = Marshal.PtrToStringAnsi(ptr);
            }
            return result;
        }

        public CURLE GetInfo(CURLINFO info, out CurlSlist value)
        {
            value = null;
            IntPtr ptr = IntPtr.Zero;
            var result = Lib.curl_easy_getinfo_ptr(easyPtr, info, ref ptr);
            value = new CurlSlist(ptr);
            return result;
        }
        #endregion

        public CURLE Perform()
        {
            CURLE result = (CURLE)(-1);
            if (!running)
            {
                running = true;
                retryCount = maxRetryCount;

                while (true)
                {
                    Prepare();
                    result = Lib.curl_easy_perform(easyPtr);
                    var done = ProcessResponse(result);
                    if (done || --retryCount < 0)
                    {
                        Dump();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(retryInterval);
                    }
                }

                running = false;
            }
            else
            {
                CurlLog.LogError("Can't preform a running handle again!");
            }

            return result;
        }

        private readonly HttpClient client = new HttpClient();

        public async Task<CURLE> PerformAsync()
        {
            return await Task.Run(Perform);
        }

        public void MultiPerform(CurlMulti _multi = null)
        {
            if (!running)
            {
                running = true;
                retryCount = maxRetryCount;
                multi = _multi ?? CurlMultiUpdater.Instance.DefaultMulti;

                Prepare();
                multi.AddEasy(this);
            }
            else
            {
                CurlLog.LogError("Can't preform a running handle again!");
            }
        }

        public void Abort()
        {
            if (multi != null)
            {
                multi.RemoveEasy(this);
                multi = null;
            }

            if (easyPtr != IntPtr.Zero)
            {
                OnComplete(CURLE.ABORTED_BY_CALLBACK);
                CloseStreams();
                CleanUp();
            }
        }

        private void OnComplete(CURLE result)
        {
            if (performCallback != null)
            {
                performCallback(result, this);
                performCallback = null;
            }
            running = false;
        }

        private void OnProgress(long dltotal, long dlnow, long ultotal, long ulnow)
        {
            if (progressCallback != null)
            {
                progressCallback(dltotal, dlnow, ultotal, ulnow, this);
            }
        }

        internal void OnMultiPerform(CURLE result, CurlMulti multi)
        {
            var done = ProcessResponse(result);

            if (done || --retryCount < 0)
            {
                Dump();

                OnComplete(result);

            }
            else
            {
                Thread.Sleep(retryInterval);
                Prepare();
                multi.AddEasy(this);
            }
        }

        private void Prepare()
        {
            try
            {
                status = 0;
                message = null;

                thisHandle = GCHandle.Alloc(this);

                SetOpt(CURLOPT.URL, uri.AbsoluteUri);

                var upperMethod = method.ToUpper();
                switch (upperMethod)
                {
                    case "GET":
                        SetOpt(CURLOPT.HTTPGET, true);
                        break;
                    case "HEAD":
                        SetOpt(CURLOPT.NOBODY, true);
                        break;
                    case "POST":
                        SetOpt(CURLOPT.POST, true);
                        break;
                    default:
                        SetOpt(CURLOPT.CUSTOMREQUEST, method);
                        break;
                }

                SetOpt(CURLOPT.HTTP_VERSION, (long)HTTPVersion.VERSION_2TLS);
                SetOpt(CURLOPT.PIPEWAIT, true);

                SetOpt(CURLOPT.SSL_VERIFYHOST, !insecure);
                SetOpt(CURLOPT.SSL_VERIFYPEER, !insecure);

                // Ca cert path
                SetOpt(CURLOPT.CAINFO_BLOB, cacert);

                // Fill request header
                var requestHeader = new CurlSlist(IntPtr.Zero);
                if (disableExpect)
                {
                    requestHeader.Append("Expect:");
                }
                requestHeader.Append($"Content-Type:{contentType}");
                if (userHeader != null)
                {
                    foreach (var entry in userHeader)
                    {
                        requestHeader.Append(entry.Key + ":" + entry.Value);
                    }
                }

                SetOpt(CURLOPT.HTTPHEADER, (IntPtr)requestHeader);
                // Fill request body
                if (outData != null && outData.Length > 0)
                {
                    SetOpt(CURLOPT.POSTFIELDS, outData);
                    SetOpt(CURLOPT.POSTFIELDSIZE, outData.Length);
                }

                // Handle response header
                responseHeaderStream = new MemoryStream();
                SetOpt(CURLOPT.HEADERFUNCTION, (Delegates.HeaderFunction)HeaderFunction);
                SetOpt(CURLOPT.HEADERDATA, (IntPtr)thisHandle);

                bool rangeRequest = rangeStart > 0 || rangeEnd > 0;

                if (rangeRequest)
                {
                    if (rangeEnd == 0) SetOpt(CURLOPT.RANGE, $"{rangeStart}-");
                    else SetOpt(CURLOPT.RANGE, $"{rangeStart}-{rangeEnd}");
                }
                else
                {
                    SetOpt(CURLOPT.RANGE, IntPtr.Zero);
                }

                // Handle response body
                recievedDataLength = 0;
                if (string.IsNullOrEmpty(outputPath))
                {
                    responseBodyStream = new MemoryStream();
                }
                else
                {
                    var dir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    responseBodyStream = new FileStream(outputPath, rangeRequest ? FileMode.Append : FileMode.OpenOrCreate);
                }
                SetOpt(CURLOPT.WRITEFUNCTION, (Delegates.WriteFunction)WriteFunction);
                SetOpt(CURLOPT.WRITEDATA, (IntPtr)thisHandle);

                // Debug
                if (debug)
                {
                    outHeader = null;
                    SetOpt(CURLOPT.VERBOSE, true);
                    SetOpt(CURLOPT.DEBUGFUNCTION, DebugFunction);
                    SetOpt(CURLOPT.DEBUGDATA, (IntPtr)thisHandle);
                }
                else
                {
                    SetOpt(CURLOPT.VERBOSE, false);
                    SetOpt(CURLOPT.DEBUGFUNCTION, IntPtr.Zero);
                    SetOpt(CURLOPT.DEBUGDATA, IntPtr.Zero);
                }

                if (progressCallback != null)
                {
                    SetOpt(CURLOPT.NOPROGRESS, false);
                    SetOpt(CURLOPT.XFERINFOFUNCTION, ProgressFunction);
                    SetOpt(CURLOPT.XFERINFODATA, (IntPtr)thisHandle);
                }
                else
                {
                    SetOpt(CURLOPT.NOPROGRESS, true);
                    SetOpt(CURLOPT.XFERINFOFUNCTION, IntPtr.Zero);
                    SetOpt(CURLOPT.XFERINFODATA, IntPtr.Zero);
                }

                // Timeout
                SetOpt(CURLOPT.CONNECTTIMEOUT_MS, connectionTimeout);
                SetOpt(CURLOPT.TIMEOUT_MS, timeout);
                SetOpt(CURLOPT.LOW_SPEED_LIMIT, lowSpeedLimit);
                SetOpt(CURLOPT.LOW_SPEED_TIME, lowSpeedTimeout);

                // Speed limitation
                SetOpt(CURLOPT.MAX_SEND_SPEED_LARGE, outSpeedLimit);
                SetOpt(CURLOPT.MAX_RECV_SPEED_LARGE, inSpeedLimit);
            }
            catch (Exception e)
            {
                CurlLog.LogError("Unexpected exception: " + e);
            }
        }

        private void CloseStreams()
        {
            if (responseHeaderStream != null)
            {
                responseHeaderStream.Close();
                responseHeaderStream = null;
            }
            if (responseBodyStream != null)
            {
                responseBodyStream.Close();
                responseBodyStream = null;
            }
        }

        private bool ProcessResponse(CURLE result)
        {
            var done = false;

            try
            {
                thisHandle.Free();

                if (result == CURLE.OK)
                {
                    responseHeaderStream.Position = 0;
                    var sr = new StreamReader(responseHeaderStream);

                    // Handle first line
                    {
                        var line = sr.ReadLine();
                        var index = line.IndexOf(' ');
                        httpVersion = line.Substring(0, index);
                        var nextIndex = line.IndexOf(' ', index + 1);
                        if (int.TryParse(line.Substring(index + 1, nextIndex - index), out var _status))
                        {
                            status = _status;
                        }
                        message = line.Substring(nextIndex + 1);
                    }

                    inHeader = new HeaderDict();

                    while (true)
                    {
                        var line = sr.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            var index = line.IndexOf(':');
                            var key = line.Substring(0, index).Trim();
                            var value = line.Substring(index + 1).Trim();
                            inHeader[key] = value;
                        }
                        else
                        {
                            break;
                        }
                    }

                    var ms = responseBodyStream as MemoryStream;
                    if (ms != null)
                    {
                        if (inHeader.TryGetValue("Content-Encoding", out string contentEncoding))
                        {
                            inData = DecompressContentStream(ms, contentEncoding);
                        }
                        else
                        {
                            inData = ms.ToArray();
                        }
                    }

                    if (status / 100 == 3)
                    {
                        if (followRedirect && GetInfo(CURLINFO.REDIRECT_URL, out string location) == CURLE.OK)
                        {
                            uri = new Uri(location);
                        }
                        else
                        {
                            done = true;
                        }
                    }
                    else
                    {
                        done = true;
                    }
                }
                else
                {
                    CurlLog.LogWarning($"Failed to request: {uri}, reason: {result}");
                }

                CloseStreams();
            }
            catch (Exception e)
            {
                CurlLog.LogError("Unexpected exception: " + e);
            }


            return done;
        }

        private byte[] DecompressContentStream(MemoryStream contentStream, string contentEncoding)
        {
            contentStream.Seek(0, SeekOrigin.Begin);

            if (contentEncoding == "gzip")
            {
                // https://stackoverflow.com/a/39157149/3553314
                using (var decompressionStream = new GZipStream(contentStream, CompressionMode.Decompress))
                using (var outputStream = new MemoryStream())
                {
                    decompressionStream.CopyTo(outputStream);
                    return outputStream.ToArray();
                }
            }
            else if (contentEncoding == "")
            {
                return contentStream.ToArray();
            }
            else
            {
                CurlLog.LogError($"Not supported Content-Encoding: {contentEncoding}");
                return contentStream.ToArray();
            }
        }

        private void Dump()
        {
            if (!debug)
            {
                return;
            }

            try
            {
                var sb = new StringBuilder();

                GetInfo(CURLINFO.EFFECTIVE_URL, out string effectiveUrl);
                GetInfo(CURLINFO.TOTAL_TIME, out double time);
                GetInfo(CURLINFO.PRIMARY_IP, out string ip);

#if UNITY_EDITOR
                var colorize = true;
#else
	            var colorize = false;
#endif

                if (colorize)
                {
                    sb.AppendLine($"<color={((status >= 200 && status <= 299) ? "green" : "red")}><b>[{method.ToUpper()}]</b></color> {effectiveUrl}({ip}) [{httpVersion} {status} {message}] [{outDataLength}({(outData != null ? outData.Length : 0)}) | {inDataLength}({(inData != null ? inData.Length : 0)}) | {time * 1000} ms]");
                }
                else
                {
                    sb.AppendLine($"[{method.ToUpper()}] {effectiveUrl}({ip}) [{httpVersion} {status} {message}] [{outDataLength}({(outData != null ? outData.Length : 0)}) | {inDataLength}({(inData != null ? inData.Length : 0)}) | {time * 1000} ms]");
                }

                if (debug)
                {
                    if (outHeader != null)
                    {
                        if (colorize) sb.AppendLine("<b><color=lightblue>Request Headers</color></b>");
                        else sb.AppendLine("-- Request Headers --");

                        foreach (var entry in outHeader)
                        {
                            if (colorize) sb.AppendLine($"<b><color=silver>[{entry.Key}]</color></b> {entry.Value}");
                            else sb.AppendLine($"[{entry.Key}] {entry.Value}");
                        }
                    }

                    if (outData != null && outData.Length > 0)
                    {
                        if (colorize) sb.AppendLine($"<b><color=lightblue>Request Body</color></b> [ {outData.Length} ]");
                        else sb.AppendLine($"-- Request Body -- [ {outData.Length} ]");

                        string outDataString = decoder?.DecodeOutData(this) ?? Encoding.UTF8.GetString(outData, 0, Math.Min(outData.Length, 0x400));

                        sb.AppendLine(outDataString);
                    }

                    if (inHeader != null)
                    {
                        if (colorize) sb.AppendLine("<b><color=lightblue>Response Headers</color></b>");
                        else sb.AppendLine("-- Response Headers --");
                        foreach (var entry in inHeader)
                        {
                            if (colorize) sb.AppendLine($"<b><color=silver>[{entry.Key}]</color></b> {entry.Value}");
                            else sb.AppendLine($"[{entry.Key}] {entry.Value}");
                        }
                    }

                    if (inData != null && inData.Length > 0)
                    {
                        if (colorize) sb.AppendLine($"<b><color=lightblue>Response Body</color></b> [ {inData.Length} ]");
                        else sb.AppendLine($"-- Response Body -- [ {inData.Length} ]");

                        string inDataString = decoder?.DecodeInData(this) ?? Encoding.UTF8.GetString(inData, 0, Math.Min(inData.Length, 0x400));

                        sb.AppendLine(inDataString);
                    }
                }
                CurlLog.Log(sb.ToString());
            }
            catch (Exception e)
            {
                CurlLog.LogError("Unexpected exception: " + e);
            }
        }

        public Dictionary<string, string> GetAllRequestHeaders()
        {
            return userHeader;
        }

        public string GetRequestHeader(string key)
        {
            string value = null;
            if (userHeader != null)
            {
                userHeader.TryGetValue(key, out value);
            }
            return value;
        }

        public void SetHeader(string key, string value)
        {
            if (userHeader == null)
            {
                userHeader = new HeaderDict();
            }
            userHeader[key] = value;
        }

        public Dictionary<string, string> GetAllResponseHeaders()
        {
            return inHeader;
        }

        public string GetResponseHeader(string key)
        {
            inHeader.TryGetValue(key, out var value);
            return value;
        }

        [AOT.MonoPInvokeCallback(typeof(Delegates.HeaderFunction))]
        private static int HeaderFunction(IntPtr ptr, int size, int nmemb, IntPtr userdata)
        {
            size = size * nmemb;
            var thiz = ((GCHandle)userdata).Target as CurlEasy;
#if ALLOW_UNSAFE
            unsafe
            {
                var ums = new UnmanagedMemoryStream((byte*)ptr, (int)size);
                ums.CopyTo(thiz.responseHeaderStream);
            }
#else
            var bytes = thiz.AcquireBuffer(size);
            Marshal.Copy(ptr, bytes, 0, (int)size);
            thiz.responseHeaderStream.Write(bytes, 0, (int)size);
#endif

            return size;
        }

        [AOT.MonoPInvokeCallback(typeof(Delegates.WriteFunction))]
        private static int WriteFunction(IntPtr ptr, int size, int nmemb, IntPtr userdata)
        {
            size = size * nmemb;
            var thiz = ((GCHandle)userdata).Target as CurlEasy;
#if ALLOW_UNSAFE
            unsafe
            {
                var ums = new UnmanagedMemoryStream((byte*)ptr, (int)size);
                ums.CopyTo(thiz.responseBodyStream);
            }
#else
            var bytes = thiz.AcquireBuffer(size);
            Marshal.Copy(ptr, bytes, 0, (int)size);
            thiz.responseBodyStream.Write(bytes, 0, (int)size);
#endif
            thiz.recievedDataLength += size;

            return size;
        }

        [AOT.MonoPInvokeCallback(typeof(Delegates.DebugFunction))]
        private static int DebugFunction(IntPtr ptr, CURLINFODEBUG type, IntPtr data, int size, IntPtr userdata)
        {
            if (type == CURLINFODEBUG.HEADER_OUT)
            {
                StreamReader sr = null;
                var thiz = ((GCHandle)userdata).Target as CurlEasy;
#if ALLOW_UNSAFE
                unsafe
                {
                    var ums = new UnmanagedMemoryStream((byte*)data, (int)size);
                    sr = new StreamReader(ums);
                }
#else
                var bytes = thiz.AcquireBuffer(size);
                Marshal.Copy(data, bytes, 0, (int)size);
                sr = new StreamReader(new MemoryStream(bytes, 0, (int)size));
#endif
                // Handle first line
                var firstLine = sr.ReadLine();

                while (true)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var index = line.IndexOf(':');
                        if (index >= 0)
                        {
                            if (thiz.outHeader == null) thiz.outHeader = new HeaderDict();
                            var key = line.Substring(0, index).Trim();
                            var value = line.Substring(index + 1).Trim();
                            thiz.outHeader[key] = value;
                        }
                        else
                        {
                            CurlLog.LogWarning($"Invalid header: {line}");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return 0;
        }

        [AOT.MonoPInvokeCallback(typeof(Delegates.ProgressFunction))]
        private static int ProgressFunction(IntPtr clientp, long dltotal, long dlnow, long ultotal, long ulnow)
        {
            var thiz = ((GCHandle)clientp).Target as CurlEasy;
            thiz.OnProgress(dltotal, dlnow, ultotal, ulnow);
            return 0;
        }

        public string Escape(string data)
        {
            string result = null;
            var ptr = Lib.curl_easy_escape(easyPtr, data);
            if (ptr != IntPtr.Zero)
            {
                result = Marshal.PtrToStringAnsi(ptr);
                Lib.curl_free(ptr);
            }
            return result;
        }

        public string Unescape(string data)
        {
            string result = null;
            var ptr = Lib.curl_easy_unescape(easyPtr, data);
            if (ptr != IntPtr.Zero)
            {
                result = Marshal.PtrToStringAnsi(ptr);
                Lib.curl_free(ptr);
            }
            return result;
        }

        public static explicit operator IntPtr(CurlEasy easy)
        {
            return easy.easyPtr;
        }

        public static explicit operator CurlEasy(IntPtr ptr)
        {
            return new CurlEasy(ptr);
        }
    }
}
