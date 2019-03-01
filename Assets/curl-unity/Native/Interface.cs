using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CurlUnity
{
    public static class Delegates
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int curl_writedata_function(IntPtr ptr, int sz, int nmemb, IntPtr userdata);
    }
    public static class Interface
    {
#if UNITY_IPHONE
    public const string LIB_NAME = "__Internal";
#else
        public const string LIB_NAME = "curl";
#endif
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_global_init(long flags);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_init();

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr curl, CURLOPT option, IntPtr arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr curl, CURLOPT option, string arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr curl, CURLOPT option, byte[] arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr curl, CURLOPT option, bool arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr curl, CURLOPT option, long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr curl, CURLOPT option, Delegates.curl_writedata_function func);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_perform(IntPtr curl);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_cleanup(IntPtr curl);
    }
}