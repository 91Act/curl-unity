using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CurlUnity
{
    unsafe public class Curl : MonoBehaviour
    {
        // Start is called before the first frame update
        static int OnWriteData(IntPtr ptr, int sz, int nmemb, IntPtr userdata)
        {
            var size = sz * nmemb;
            var ums = new UnmanagedMemoryStream((byte*)ptr.ToPointer(), size);
            var msHandle = (GCHandle)userdata;
            var ms = msHandle.Target as MemoryStream;
            ums.CopyTo(ms);
            return size;
        }

        void Start()
        {
            Interface.curl_global_init((long)CURLGLOBAL.ALL);

            var handle = Interface.curl_easy_init();
            var ms = new MemoryStream();
            var msHandle = GCHandle.Alloc(ms, GCHandleType.Pinned);

            Debug.Log(Interface.curl_easy_setopt(handle, CURLOPT.URL, @"https://nghttp2.org"));
            Debug.Log(Interface.curl_easy_setopt(handle, CURLOPT.HTTP_VERSION, (int)HTTPVersion.VERSION_2_0));
            Debug.Log(Interface.curl_easy_setopt(handle, CURLOPT.SSL_VERIFYPEER, false));
            Debug.Log(Interface.curl_easy_setopt(handle, CURLOPT.SSL_VERIFYHOST, false));
            Debug.Log(Interface.curl_easy_setopt(handle, CURLOPT.HEADER, true));
            Debug.Log(Interface.curl_easy_setopt(handle, CURLOPT.WRITEFUNCTION, OnWriteData));
            Debug.Log(Interface.curl_easy_setopt(handle, CURLOPT.WRITEDATA, (IntPtr)msHandle));
            Interface.curl_easy_perform(handle);

            msHandle.Free();

            ms.Position = 0;
            Debug.Log(new StreamReader(ms).ReadToEnd());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}