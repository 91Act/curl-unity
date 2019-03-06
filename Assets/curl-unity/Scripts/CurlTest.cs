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
            var easy = new CurlEasy();
            easy.url = "https://nghttp2.org";
            easy.useHttp2 = true;
            easy.timeout = 5000;

            var multi = new CurlMulti();
            easy.MultiPerform(multi, OnPerformCallback);
        }

        void OnPerformCallback(CURLE result, CurlEasy easy)
        {
            if (result == CURLE.OK)
            {
                Debug.Log(Encoding.UTF8.GetString(easy.inData));
            }
        }
    }
}