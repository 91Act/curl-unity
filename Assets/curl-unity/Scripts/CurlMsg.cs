using System;
using System.Runtime.InteropServices;

namespace CurlUnity
{

    [StructLayout(LayoutKind.Sequential)]
    struct __curl_msg
    {
        public CURLMSG msg;
        public IntPtr easyHandle;
        public CURLE result;
    }

    public class CurlMsg
    {
        private IntPtr ptr;

        private __curl_msg internalMsg;

        public CURLMSG message { get { return internalMsg.msg; } }
        public IntPtr easyPtr { get { return internalMsg.easyHandle; } }
        public CURLE result { get { return internalMsg.result; } }

        public CurlMsg(IntPtr _ptr)
        {
            ptr = _ptr;
            if (ptr != IntPtr.Zero)
            {
                internalMsg = Marshal.PtrToStructure<__curl_msg>(ptr);
            }
        }

        public static explicit operator IntPtr(CurlMsg slist)
        {
            return slist.ptr;
        }

        public static explicit operator CurlMsg(IntPtr ptr)
        {
            return new CurlMsg(ptr);
        }
    }
}