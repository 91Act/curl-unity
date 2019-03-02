using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CurlUnity
{
    public class CurlTest : MonoBehaviour
    {
        void Start()
        {
            Lib.curl_global_init((long)CURLGLOBAL.ALL);

            var curl = new CurlEasy();

            curl.SetOpt(CURLOPT.URL, @"https://nghttp2.org");
            curl.SetOpt(CURLOPT.HTTP_VERSION, (int)HTTPVersion.VERSION_2_0);
            // TODO: Resolve the CA path for devices
            curl.SetOpt(CURLOPT.CAINFO, Path.GetFullPath("Assets/curl-unity/cacert.pem"));
            //curl.SetOpt(CURLOPT.SSL_VERIFYPEER, false);
            //curl.SetOpt(CURLOPT.SSL_VERIFYHOST, false);

            if (curl.Perform() == CURLE.OK)
            {
                curl.GetInfo(CURLINFO.HEADER_SIZE, out long headerSize);
                Debug.Log("Header size: " + headerSize);

                curl.GetInfo(CURLINFO.SPEED_DOWNLOAD, out double downloadSpeed);
                Debug.Log("Download speed: " + downloadSpeed);

                curl.GetInfo(CURLINFO.CONTENT_TYPE, out string contentType);
                Debug.Log("Content type: " + contentType);

                curl.GetInfo(CURLINFO.SSL_ENGINES, out CurlSlist sslEngines);

                Debug.Log("SSL engines: " + string.Join(", ", sslEngines.GetStrings()));

                foreach(var entry in curl.GetAllResponseHeaders())
                {
                    Debug.Log("Header: " + entry.Key + " -> " + entry.Value);
                }

                var sr = new StreamReader(curl.GetResponseBody());
                Debug.Log(sr.ReadToEnd());
            }
        }
    }
}