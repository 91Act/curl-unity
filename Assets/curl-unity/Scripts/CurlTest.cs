using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace CurlUnity
{
    public class CurlTest : MonoBehaviour
    {
        private CurlEasy m_curl;

        async void Start()
        {
            m_curl = new CurlEasy();
            m_curl.url = @"http://www.qq.com";
            m_curl.debug = true;
            m_curl.useHttp2 = true;
            m_curl.timeout = 5000;
            m_curl.outData = Encoding.UTF8.GetBytes("");

            var result = await m_curl.Perform();
            if (result != CURLE.OK)
            {
                Debug.LogWarning("Perform failed: " + result);
            }
        }

        private void Update()
        {
            if (m_curl != null)
            {
                if (m_curl.running) Debug.Log("Running...");
                else
                {
                    m_curl.Dispose();
                    m_curl = null;
                }
            }
        }
    }
}