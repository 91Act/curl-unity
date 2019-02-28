using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CurlUnity
{
    public static class Interface
    {
#if UNITY_IPHONE
    public const string LIB_NAME = "__Internal";
#else
        public const string LIB_NAME = "curl";
#endif

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_global_init(long flags);
    }
}