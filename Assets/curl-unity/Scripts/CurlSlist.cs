using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CurlUnity
{
    [StructLayout(LayoutKind.Sequential)]
    struct __curl_slist
    {
        public IntPtr data;
        public IntPtr next;
    }

    public class CurlSlist : IDisposable
    {
        private IntPtr ptr;

        public CurlSlist(IntPtr _ptr)
        {
            ptr = _ptr;
        }

        public void Dispose()
        {
            unsafe
            {
                if (ptr != IntPtr.Zero)
                {
                    Lib.curl_slist_free_all((IntPtr)(ptr));
                    ptr = IntPtr.Zero;
                }
            }
        }

        public List<string> GetStrings()
        {
            var result = new List<string>();
            unsafe
            {
                var iter = (__curl_slist*)(ptr);
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
                var head = (__curl_slist*)(ptr);
                ptr = Lib.curl_slist_append((IntPtr)head, value);
            }
        }

        public static explicit operator IntPtr(CurlSlist slist)
        {
            return slist.ptr;
        }

        public static explicit operator CurlSlist(IntPtr ptr)
        {
            return new CurlSlist(ptr);
        }
    }
}