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
            if (ptr != IntPtr.Zero)
            {
                Lib.curl_slist_free_all(ptr);
                ptr = IntPtr.Zero;
            }
        }

        public List<string> GetStrings()
        {
            var result = new List<string>();

            var iter = ptr;
            while (iter != IntPtr.Zero)
            {
                var slist = Marshal.PtrToStructure<__curl_slist>(iter);
                result.Add(Marshal.PtrToStringAnsi(slist.data));
                iter = slist.next;
            }

            return result;
        }

        public void Append(string value)
        {
            ptr = Lib.curl_slist_append(ptr, value);
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