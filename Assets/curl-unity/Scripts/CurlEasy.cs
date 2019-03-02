using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace CurlUnity
{
    public class CurlEasy : IDisposable
    {
        private IntPtr m_curl;
        private Stream m_responseHeaderStream;
        private Stream m_responseBodyStream;
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
            return Lib.curl_easy_setopt(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, string value)
        {
            return Lib.curl_easy_setopt(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, byte[] value)
        {
            return Lib.curl_easy_setopt(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, bool value)
        {
            return Lib.curl_easy_setopt(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, long value)
        {
            return Lib.curl_easy_setopt(m_curl, options, value);
        }

        public CURLE SetOpt(CURLOPT options, Delegates.curl_writedata_function value)
        {
            return Lib.curl_easy_setopt(m_curl, options, value);
        }

        #endregion
        #region GetInfo
        public CURLE GetInfo(CURLINFO info, out long value)
        {
            value = 0;
            return Lib.curl_easy_getinfo(m_curl, info, ref value);
        }

        public CURLE GetInfo(CURLINFO info, out double value)
        {
            value = 0;
            return Lib.curl_easy_getinfo(m_curl, info, ref value);
        }

        public CURLE GetInfo(CURLINFO info, out string value)
        {
            value = null;
            return Lib.curl_easy_getinfo(m_curl, info, ref value);
        }

        public CURLE GetInfo(CURLINFO info, out CurlSlist value)
        {
            value = null;
            IntPtr ptr = IntPtr.Zero;
            var result = Lib.curl_easy_getinfo(m_curl, info, ref ptr);
            if (ptr != IntPtr.Zero)
            {
                value = new CurlSlist(ptr);
            }
            return result;
        }
        #endregion

        public CURLE Perform()
        {
            m_responseHeaderStream = new MemoryStream();
            m_responseBodyStream = new MemoryStream();

            SetOpt(CURLOPT.HEADERFUNCTION, WriteHeader);
            SetOpt(CURLOPT.WRITEFUNCTION, WriteData);

            var result = Lib.curl_easy_perform(m_curl);

            m_responseHeader = new Dictionary<string, string>();

            m_responseHeaderStream.Position = 0;
            var sr = new StreamReader(m_responseHeaderStream);

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

        public Stream GetResponseBody(bool rewind = true)
        {
            if (rewind)
            {
                m_responseBodyStream.Position = 0;
            }
            return m_responseBodyStream;
        }

        private int WriteHeader(IntPtr ptr, int sz, int nmemb, IntPtr userdata)
        {
            unsafe
            {
                var size = sz * nmemb;
                var ums = new UnmanagedMemoryStream((byte*)ptr, size);
                ums.CopyTo(m_responseHeaderStream);
                return size;
            }
        }

        private int WriteData(IntPtr ptr, int sz, int nmemb, IntPtr userdata)
        {
            unsafe
            {
                var size = sz * nmemb;
                var ums = new UnmanagedMemoryStream((byte*)ptr, size);
                ums.CopyTo(m_responseBodyStream);
                return size;
            }
        }
    }
}