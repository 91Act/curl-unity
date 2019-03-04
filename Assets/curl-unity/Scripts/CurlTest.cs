using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace CurlUnity
{
    public class CurlTest : MonoBehaviour
    {
        void Start()
        {
            Lib.curl_global_init((long)CURLGLOBAL.ALL);

            var curl = new CurlEasy();

            curl.SetOpt(CURLOPT.VERBOSE, true);
            curl.SetOpt(CURLOPT.URL, @"https://nghttp2.org");
            var capath = Path.Combine(Application.persistentDataPath, "cacert");
            if (!File.Exists(capath))
            {
                File.WriteAllBytes(capath, Resources.Load<TextAsset>("cacert").bytes);
            }
            curl.SetOpt(CURLOPT.HTTP_VERSION, (int)HTTPVersion.VERSION_2_0);
            curl.SetOpt(CURLOPT.CAINFO, capath);

            var result = curl.Perform();
            if (result == CURLE.OK)
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

                Debug.Log(Encoding.UTF8.GetString(curl.GetResponseBody()));
            }
            else
            {
                Debug.LogWarning("Perform failed: " + result);
            }
        }
    }
}