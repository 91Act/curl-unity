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

    public static class Lib
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        public const string LIB_NAME = "__Internal";
#else
        public const string LIB_NAME = "curl";
#endif

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_global_init(long flags);

        #region easy interfaces
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_init();
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_cleanup(IntPtr curl);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_int(IntPtr curl, CURLOPT option, long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_int(IntPtr curl, CURLOPT option, bool arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_str(IntPtr curl, CURLOPT option, string arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr curl, CURLOPT option, IntPtr arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr curl, CURLOPT option, byte[] arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr curl, CURLOPT option, Delegates.curl_writedata_function arg);
        
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_perform(IntPtr curl);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr curl, CURLINFO info, ref long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr curl, CURLINFO info, ref double arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr curl, CURLINFO info, ref IntPtr arg);
        
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_escape(IntPtr curl, string data, long length = 0);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_unescape(IntPtr curl, string data, long length = 0);

        #endregion
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_slist_append(IntPtr arg, string data);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_slist_free_all(IntPtr arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_free(IntPtr data);
    }
}