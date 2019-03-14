using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CurlUnity
{
    public class CurlEasy : IDisposable
    {
        public delegate void PerformCallback(CURLE result, CurlEasy easy);

        public Uri uri { get; set; }
        public string method { get; set; } = "GET";
        public string contentType { get; set; } = "application/text";
        public string outputPath { get; set; }
        public int timeout { get; set; } = 0;
        public int connectionTimeout { get; set; } = 5000;
        public int maxRetryCount { get; set; } = 5;
        public bool forceHttp2 { get; set; }
        public bool insecure { get; set; }
        public byte[] outData { get; set; }
        public byte[] inData { get; private set; }
        public string httpVersion { get; private set; }
        public int status { get; private set; }
        public string message { get; private set; }
        public bool running { get; private set; }
        public PerformCallback performCallback { get; set; }
        public bool debug { get; set; }

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

        public long recievedDataLength
        {
            get
            {
                return responseBodyStream != null ? responseBodyStream.Length : 0;
            }
        }

        private IntPtr easyPtr;
        private CurlMulti multi;
        private int retryCount;
        private Dictionary<string, string> userHeader;
        private Dictionary<string, string> outHeader;
        private Dictionary<string, string> inHeader;

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

        private static string s_capath;

        static CurlEasy()
        {
            s_capath = Path.Combine(Application.persistentDataPath, "cacert");
            if (!File.Exists(s_capath))
            {
                File.WriteAllBytes(s_capath, Resources.Load<TextAsset>("cacert").bytes);
            }
            Lib.curl_global_init((long)CURLGLOBAL.ALL);
        }

        public CurlEasy(IntPtr ptr = default(IntPtr))
        {
            if (ptr != IntPtr.Zero)
            {
                easyPtr = ptr;
            }
            else
            {
                easyPtr = Lib.curl_easy_init();
            }
        }

        public void CleanUp()
        {
            if (easyPtr != IntPtr.Zero)
            {
                Lib.curl_easy_cleanup(easyPtr);
                easyPtr = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            CleanUp();
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
        public CURLE SetOpt(CURLOPT options, long value)
        {
            return Lib.curl_easy_setopt_int(easyPtr, options, value);
        }

        public CURLE SetOpt(CURLOPT options, bool value)
        {
            return Lib.curl_easy_setopt_int(easyPtr, options, value);
        }

        public CURLE SetOpt(CURLOPT options, string value)
        {
            return Lib.curl_easy_setopt_str(easyPtr, options, value);
        }

        public CURLE SetOpt(CURLOPT options, IntPtr value)
        {
            return Lib.curl_easy_setopt_ptr(easyPtr, options, value);
        }

        public CURLE SetOpt(CURLOPT options, byte[] value)
        {
            return Lib.curl_easy_setopt_ptr(easyPtr, options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.WriteFunction value)
        {
            return Lib.curl_easy_setopt_ptr(easyPtr, options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.HeaderFunction value)
        {
            return Lib.curl_easy_setopt_ptr(easyPtr, options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.DebugFunction value)
        {
            return Lib.curl_easy_setopt_ptr(easyPtr, options, value);
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
                        if (debug) Dump();
                        break;
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

        public async Task<CURLE> PerformAsync()
        {
            return await Task.Run(Perform);
        }

        public void MultiPerform(CurlMulti _multi)
        {
            if (!running)
            {
                running = true;
                retryCount = maxRetryCount;
                multi = _multi;

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
            performCallback = null;
        }

        public void OnMultiPerform(CURLE result, CurlMulti multi)
        {
            var done = ProcessResponse(result);

            if (done || --retryCount < 0)
            {
                if (debug) Dump();
                performCallback?.Invoke(result, this);
                performCallback = null;
                running = false;
            }
            else
            {
                Prepare();
                multi.AddEasy(this);
            }
        }

        private void Prepare()
        {
            status = 0;
            message = null;

            thisHandle = GCHandle.Alloc(this);

            SetOpt(CURLOPT.URL, uri.AbsoluteUri);
            SetOpt(CURLOPT.CUSTOMREQUEST, method);

            if (forceHttp2 || uri.Scheme == "https")
            {
                SetOpt(CURLOPT.HTTP_VERSION, (long)HTTPVersion.VERSION_2_0);
                SetOpt(CURLOPT.PIPEWAIT, true);
            }

            if (insecure)
            {
                SetOpt(CURLOPT.SSL_VERIFYHOST, false);
                SetOpt(CURLOPT.SSL_VERIFYPEER, false);
            }

            // Ca cert path
            SetOpt(CURLOPT.CAINFO, s_capath);

            // Fill request header
            var requestHeader = new CurlSlist(IntPtr.Zero);
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

            // Handle response body
            if (string.IsNullOrEmpty(outputPath))
            {
                responseBodyStream = new MemoryStream();
            }
            else
            {
                var dir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                responseBodyStream = new FileStream(outputPath, FileMode.OpenOrCreate);
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

            // Timeout
            SetOpt(CURLOPT.CONNECTTIMEOUT_MS, connectionTimeout);
            SetOpt(CURLOPT.TIMEOUT_MS, timeout);
        }

        private bool ProcessResponse(CURLE result)
        {
            var done = false;

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

                inHeader = new Dictionary<string, string>();

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
                inData = ms?.ToArray();

                if (status == 200)
                {
                    done = true;
                }
                else if (status / 100 == 3)
                {
                    if (GetInfo(CURLINFO.REDIRECT_URL, out string location) == CURLE.OK)
                    {
                        uri = new Uri(location);
                    }
                }
            }
            else
            {
                CurlLog.LogWarning($"Failed to request: {uri}, reason: {result}");
            }

            responseHeaderStream.Close();
            responseHeaderStream = null;
            responseBodyStream.Close();
            responseBodyStream = null;

            return done;
        }

        private void Dump()
        {
            var sb = new StringBuilder();

            GetInfo(CURLINFO.EFFECTIVE_URL, out string effectiveUrl);
            GetInfo(CURLINFO.TOTAL_TIME, out double time);

#if UNITY_EDITOR
            var colorize = true;
#else
            var colorize = false;
#endif

            sb.AppendLine($"{effectiveUrl} [ {method.ToUpper()} ] [ {httpVersion} {status} {message} ] [ {outDataLength}({(outData != null ? outData.Length : 0)}) | {inDataLength}({(inData != null ? inData.Length : 0)}) ] [ {time * 1000} ms ]");

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
                sb.AppendLine(Encoding.UTF8.GetString(outData, 0, Math.Min(outData.Length, 0x400)));
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
                sb.AppendLine(Encoding.UTF8.GetString(inData, 0, Math.Min(inData.Length, 0x400)));
            }

            CurlLog.Log(sb.ToString());
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
                userHeader = new Dictionary<string, string>();
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
                var ums = new UnmanagedMemoryStream((byte*)ptr, size);
                ums.CopyTo(thiz.responseHeaderStream);
            }
#else
            var bytes = thiz.AcquireBuffer(size);
            Marshal.Copy(ptr, bytes, 0, size);
            thiz.responseHeaderStream.Write(bytes, 0, size);
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
                var ums = new UnmanagedMemoryStream((byte*)ptr, size);
                ums.CopyTo(thiz.responseBodyStream);
            }
#else
            var bytes = thiz.AcquireBuffer(size);
            Marshal.Copy(ptr, bytes, 0, size);
            thiz.responseBodyStream.Write(bytes, 0, size);
#endif
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
                    var ums = new UnmanagedMemoryStream((byte*)data, size);
                    sr = new StreamReader(ums);
                }
#else
                var bytes = thiz.AcquireBuffer(size);
                Marshal.Copy(data, bytes, 0, size);
                sr = new StreamReader(new MemoryStream(bytes, 0, size));
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
                            if (thiz.outHeader == null) thiz.outHeader = new Dictionary<string, string>();
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