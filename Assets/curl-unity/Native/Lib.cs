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
        public delegate int WriteFunction(IntPtr ptr, int size, int nmemb, IntPtr userdata);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int HeaderFunction(IntPtr ptr, int size, int nmemb, IntPtr userdata);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DebugFunction(IntPtr ptr, CURLINFODEBUG type, IntPtr data, int size, IntPtr userdata);
    }

    public static class Lib
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        public const string LIB_NAME = "__Internal";
#else
        public const string LIB_NAME = "curl";
#endif

        #region misc
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_global_init(long flags);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_slist_append(IntPtr arg, string data);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_slist_free_all(IntPtr arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_free(IntPtr data);
        #endregion
        #region easy interfaces
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_init();
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_cleanup(IntPtr handle);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_reset(IntPtr handle);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_duphandle(IntPtr handle);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_int(IntPtr handle, CURLOPT option, long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_int(IntPtr handle, CURLOPT option, bool arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_str(IntPtr handle, CURLOPT option, string arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr handle, CURLOPT option, IntPtr arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr handle, CURLOPT option, byte[] arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr handle, CURLOPT option, Delegates.WriteFunction arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr handle, CURLOPT option, Delegates.HeaderFunction arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr handle, CURLOPT option, Delegates.DebugFunction arg);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_perform(IntPtr handle);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr handle, CURLINFO info, ref long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr handle, CURLINFO info, ref double arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr handle, CURLINFO info, ref IntPtr arg);
        
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_escape(IntPtr handle, string data, long length = 0);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_unescape(IntPtr handle, string data, long length = 0);

        #endregion

        #region multi interface
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_init();
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLM curl_multi_cleanup(IntPtr mhandle);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_multi_add_handle(IntPtr mhandle, IntPtr ehandle);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLM curl_multi_remove_handle(IntPtr mhandle, IntPtr ehandle);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLM curl_multi_perform(IntPtr mhandle, ref long running);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_info_read(IntPtr mhandle, ref long messages);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_int(IntPtr mhandle, CURLMOPT opt, long value);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_str(IntPtr mhandle, CURLMOPT opt, string value);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_ptr(IntPtr mhandle, CURLMOPT opt, IntPtr value);
        #endregion
    }
}