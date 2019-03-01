using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CurlUnity
{
    unsafe public class Curl : MonoBehaviour
    {
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

            Interface.curl_easy_setopt(handle, CURLOPT.URL, @"https://nghttp2.org");
            Interface.curl_easy_setopt(handle, CURLOPT.HTTP_VERSION, (int)HTTPVersion.VERSION_2_0);
            Interface.curl_easy_setopt(handle, CURLOPT.SSL_VERIFYPEER, false);
            Interface.curl_easy_setopt(handle, CURLOPT.SSL_VERIFYHOST, false);
            Interface.curl_easy_setopt(handle, CURLOPT.HEADER, true);
            Interface.curl_easy_setopt(handle, CURLOPT.WRITEFUNCTION, OnWriteData);
            Interface.curl_easy_setopt(handle, CURLOPT.WRITEDATA, (IntPtr)msHandle);

            if (Interface.curl_easy_perform(handle) == CURLE.OK)
            {
                long headerSize = 0;
                Interface.curl_easy_getinfo(handle, CURLINFO.HEADER_SIZE, ref headerSize);
                Debug.Log("Header size: " + headerSize);

                string contentType = null;
                Interface.curl_easy_getinfo(handle, CURLINFO.CONTENT_TYPE, ref contentType);
                Debug.Log("Content type: " + contentType);

                double downloadSpeed = 0;
                Interface.curl_easy_getinfo(handle, CURLINFO.SPEED_DOWNLOAD, ref downloadSpeed);
                Debug.Log("Download speed: " + downloadSpeed);

                CURLSlist* slist;
                if (Interface.curl_easy_getinfo(handle, CURLINFO.SSL_ENGINES, &slist) == CURLE.OK)
                {
                    var engines = CURLSlist.ToStings(slist);
                    Interface.curl_slist_free_all(slist);

                    Debug.Log("SSL engines: " + string.Join(", ", engines));
                }

                ms.Position = 0;
                Debug.Log(new StreamReader(ms).ReadToEnd());
            }

            msHandle.Free();
        }
    }
}