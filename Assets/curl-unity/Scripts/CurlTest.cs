using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace CurlUnity
{
    public class CurlTest : MonoBehaviour
    {
        private CurlMulti m_multi;

        private void MakeRequest(int id)
        {
            var easy = new CurlEasy();
            easy.url = $"http://nghttp2.org?id={id}";
            easy.debug = true;
            easy.useHttp2 = true;
            easy.timeout = 5000;

            easy.Perform(m_multi, OnPerformCallback);
        }

        private void OnPerformCallback(CurlEasy easy)
        {
            Debug.Log("Perform finished: " + easy.url);
        }

        void Start()
        {
            m_multi = new CurlMulti();

            for(int i = 0; i < 10; i++)
            {
                MakeRequest(i);
            }
        }

        void Update()
        {
            m_multi.Tick();
        }
    }
}