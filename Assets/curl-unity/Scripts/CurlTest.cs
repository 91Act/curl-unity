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

            using (var curl = new CurlEasy())
            {
                curl.url = @"http://www.qq.com";
                curl.debug = true;
                curl.useHttp2 = true;
                curl.timeout = 5000;
                curl.outData = Encoding.UTF8.GetBytes("");                

                var result = curl.Perform();
                if (result != CURLE.OK)
                {
                    Debug.LogWarning("Perform failed: " + result);
                }
            }
        }
    }
}