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
            var handle = Interface.curl_easy_init();
            var ms = new MemoryStream();
            var msHandle = GCHandle.Alloc(ms, GCHandleType.Pinned);

            Interface.curl_easy_setopt(handle, CURLOPT.URL, @"https://nghttp2.org");
            Interface.curl_easy_setopt(handle, CURLOPT.HTTP_VERSION, (int)HTTPcode.Version.CURL_HTTP_VERSION_2_0);
            Interface.curl_easy_setopt(handle, CURLOPT.SSL_VERIFYPEER, false);
            Interface.curl_easy_setopt(handle, CURLOPT.SSL_VERIFYHOST, false);
            Interface.curl_easy_setopt(handle, CURLOPT.HEADER, true);
            Interface.curl_easy_setopt(handle, CURLOPT.WRITEFUNCTION, OnWriteData);
            Interface.curl_easy_setopt(handle, CURLOPT.WRITEDATA, (IntPtr)msHandle);            
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