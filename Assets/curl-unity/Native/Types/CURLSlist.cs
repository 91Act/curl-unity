using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CurlUnity
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CURLSlist
    {
        public IntPtr data;
        public IntPtr next;

        unsafe public static List<string> ToStings(CURLSlist* first)
        {
            var result = new List<string>();

            var iter = first;
            while (iter != null)
            {
                result.Add(Marshal.PtrToStringAnsi(iter->data));
                iter = (CURLSlist*)(iter->next.ToPointer());
            }

            return result;
        }
    }
}