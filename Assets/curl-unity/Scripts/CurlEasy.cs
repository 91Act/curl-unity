using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CurlUnity
{
    public class CurlEasy : IDisposable
    {
        private IntPtr m_curl;
        private byte[] m_responseBody;
        private Dictionary<string, string> m_responseHeader;

        public CurlEasy()
        {
            m_curl = Lib.curl_easy_init();
        }

        public void Dispose()
        {
            Lib.curl_easy_cleanup(m_curl);
        }

        #region SetOpt
        public CURLE SetOpt(CURLOPT options, IntPtr value)
        {
            return Lib.curl_easy_setopt_ptr(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, string value)
        {
            return Lib.curl_easy_setopt_str(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, byte[] value)
        {
            return Lib.curl_easy_setopt_ptr(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, bool value)
        {
            return Lib.curl_easy_setopt_int(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, long value)
        {
            return Lib.curl_easy_setopt_int(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.curl_writedata_function value)
        {
            return Lib.curl_easy_setopt_ptr(m_curl, options, value);
        }

        #endregion
        #region GetInfo
        public CURLE GetInfo(CURLINFO info, out long value)
        {
            value = 0;
            return Lib.curl_easy_getinfo_ptr(m_curl, info, ref value);
        }

        public CURLE GetInfo(CURLINFO info, out double value)
        {
            value = 0;
            return Lib.curl_easy_getinfo_ptr(m_curl, info, ref value);
        }

        public CURLE GetInfo(CURLINFO info, out string value)
        {
            value = null;
            IntPtr ptr = IntPtr.Zero;
            var result = Lib.curl_easy_getinfo_ptr(m_curl, info, ref ptr);
            if (ptr != IntPtr.Zero)
            {
                unsafe
                {
                    value = Marshal.PtrToStringAnsi((IntPtr)ptr.ToPointer());
                }
            }
            return result;
        }

        public CURLE GetInfo(CURLINFO info, out CurlSlist value)
        {
            value = null;
            IntPtr ptr = IntPtr.Zero;
            var result = Lib.curl_easy_getinfo_ptr(m_curl, info, ref ptr);
            value = new CurlSlist(ptr);
            return result;
        }
        #endregion

        public CURLE Perform()
        {
            var responseHeaderStream = new MemoryStream();
            var responseBodyStream = new MemoryStream();

            var headerHandle = GCHandle.Alloc(responseHeaderStream);
            var bodyHandle = GCHandle.Alloc(responseBodyStream);

            SetOpt(CURLOPT.HEADERFUNCTION, WriteData);
            SetOpt(CURLOPT.HEADERDATA, (IntPtr)headerHandle);
            SetOpt(CURLOPT.WRITEFUNCTION, WriteData);
            SetOpt(CURLOPT.WRITEDATA, (IntPtr)bodyHandle);

            var result = Lib.curl_easy_perform(m_curl);

            headerHandle.Free();
            bodyHandle.Free();

            m_responseHeader = new Dictionary<string, string>();

            responseHeaderStream.Position = 0;
            var sr = new StreamReader(responseHeaderStream);

            // Skip the first line for status code
            sr.ReadLine();

            while (true)
            {
                var line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    var entries = line.Split(':');
                    m_responseHeader.Add(entries[0].Trim(), entries[1].Trim());
                }
                else
                {
                    break;
                }
            }

            m_responseBody = responseBodyStream.ToArray();

            return result;
        }

        public string Escape(string data)
        {
            string result = null;
            var ptr = Lib.curl_easy_escape(m_curl, data);
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
            var ptr = Lib.curl_easy_unescape(m_curl, data);
            if (ptr != IntPtr.Zero)
            {
                result = Marshal.PtrToStringAnsi(ptr);
                Lib.curl_free(ptr);
            }
            return result;
        }

        public Dictionary<string, string> GetAllResponseHeaders()
        {
            return m_responseHeader;
        }

        public string GetResponseHeader(string key)
        {
            m_responseHeader.TryGetValue(key, out var value);
            return value;
        }

        public byte[] GetResponseBody(bool rewind = true)
        {
            return m_responseBody;
        }

        [AOT.MonoPInvokeCallback(typeof(Delegates.curl_writedata_function))]
        private static int WriteData(IntPtr ptr, int sz, int nmemb, IntPtr userdata)
        {
            unsafe
            {
                var size = sz * nmemb;
                var ums = new UnmanagedMemoryStream((byte*)ptr, size);
                var handle = (GCHandle)userdata;
                ums.CopyTo(handle.Target as Stream);
                return size;
            }
        }
    }
}