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
        public delegate int ProgressFunction(IntPtr clientp, int dltotal, int dlnow, int ultotal, int ulnow);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DebugFunction(IntPtr ptr, CURLINFODEBUG type, IntPtr data, int size, IntPtr userdata);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LockFunction(IntPtr ptr, CURLLOCKDATA data, CURLLOCKACCESS access, IntPtr userdata);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void UnlockFunction(IntPtr ptr, CURLLOCKDATA data, IntPtr userdata);
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
        public static extern void curl_easy_cleanup(IntPtr easyPtr);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_reset(IntPtr easyPtr);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_duphandle(IntPtr easyPtr);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_int(IntPtr easyPtr, CURLOPT option, long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_int(IntPtr easyPtr, CURLOPT option, bool arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_str(IntPtr easyPtr, CURLOPT option, string arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr easyPtr, CURLOPT option, IntPtr arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr easyPtr, CURLOPT option, byte[] arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr easyPtr, CURLOPT option, Delegates.WriteFunction arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr easyPtr, CURLOPT option, Delegates.HeaderFunction arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr easyPtr, CURLOPT option, Delegates.DebugFunction arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_setopt_ptr(IntPtr easyPtr, CURLOPT option, Delegates.ProgressFunction arg);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_perform(IntPtr easyPtr);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr easyPtr, CURLINFO info, ref long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr easyPtr, CURLINFO info, ref double arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_easy_getinfo_ptr(IntPtr easyPtr, CURLINFO info, ref IntPtr arg);
        
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_escape(IntPtr easyPtr, string data, long length = 0);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_easy_unescape(IntPtr easyPtr, string data, long length = 0);

        #endregion

        #region multi interface
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_init();
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLM curl_multi_cleanup(IntPtr multiPtr);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLE curl_multi_add_handle(IntPtr multiPtr, IntPtr easyPtr);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLM curl_multi_remove_handle(IntPtr multiPtr, IntPtr easyPtr);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLM curl_multi_perform(IntPtr multiPtr, ref long running);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_info_read(IntPtr multiPtr, ref long messages);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_int(IntPtr multiPtr, CURLMOPT opt, long value);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_int(IntPtr multiPtr, CURLMOPT opt, bool value);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_str(IntPtr multiPtr, CURLMOPT opt, string value);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_ptr(IntPtr multiPtr, CURLMOPT opt, IntPtr value);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_setopt_ptr(IntPtr multiPtr, CURLMOPT opt, byte[] value);
        #endregion

        #region share interface
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_share_init();
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_setopt_int(IntPtr sharePtr, CURLSHOPT option, long arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_setopt_int(IntPtr sharePtr, CURLSHOPT option, bool arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_setopt_str(IntPtr sharePtr, CURLSHOPT option, string arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_setopt_ptr(IntPtr sharePtr, CURLSHOPT option, IntPtr arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_setopt_ptr(IntPtr sharePtr, CURLSHOPT option, byte[] arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_setopt_ptr(IntPtr sharePtr, CURLSHOPT option, Delegates.LockFunction arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_setopt_ptr(IntPtr sharePtr, CURLSHOPT option, Delegates.UnlockFunction arg);
        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLSH curl_share_cleanup(IntPtr sharePtr);
        #endregion
    }
}