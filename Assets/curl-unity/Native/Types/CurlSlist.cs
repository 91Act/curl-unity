using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CurlUnity
{
    public class CurlSlist : IDisposable
    {
        private IntPtr m_ptr;

        public CurlSlist(IntPtr ptr)
        {
            m_ptr = ptr;
        }

        public void Dispose()
        {
            unsafe
            {
                if (m_ptr != IntPtr.Zero)
                {
                    Lib.curl_slist_free_all((IntPtr)(m_ptr));
                    m_ptr = IntPtr.Zero;
                }
            }
        }

        public List<string> GetStrings()
        {
            var result = new List<string>();
            unsafe
            {
                var iter = (__curl_slist*)(m_ptr);
                while(iter != null)
                {
                    result.Add(Marshal.PtrToStringAnsi(iter->data));
                    iter = (__curl_slist*)(iter->next);
                }
            }
            return result;
        }

        public void Append(string value)
        {
            unsafe
            {
                var head = (__curl_slist*)(m_ptr);
                m_ptr = Lib.curl_slist_append((IntPtr)head, value);
            }
        }

        public IntPtr GetPtr()
        {
            return m_ptr;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct __curl_slist
        {
            public IntPtr data;
            public IntPtr next;
        }

    }
}