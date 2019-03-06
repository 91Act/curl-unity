using System;
using System.Collections.Generic;

namespace CurlUnity
{
    public class CurlMulti : IDisposable
    {
        public delegate void MultiPerformCallback(CURLE result, CurlMulti multi);

        private IntPtr multiPtr;
        private CurlShare share;
        private Dictionary<IntPtr, CurlEasy> workingEasies = new Dictionary<IntPtr, CurlEasy>();

        public CurlMulti(IntPtr ptr = default(IntPtr))
        {
            if (ptr != IntPtr.Zero)
            {
                multiPtr = ptr;
            }
            else
            {
                multiPtr = Lib.curl_multi_init();
            }

            Lib.curl_multi_setopt_int(multiPtr, CURLMOPT.PIPELINING, (long)CURLPIPE.MULTIPLEX);

            share = new CurlShare();

            share.SetOpt(CURLSHOPT.SHARE, (long)CURLLOCKDATA.SSL_SESSION);
        }

        public void Dispose()
        {
            Lib.curl_multi_cleanup(multiPtr);
        }

        public void AddHandle(CurlEasy easy)
        {
            workingEasies[(IntPtr)easy] = easy;
            Lib.curl_multi_add_handle(multiPtr, (IntPtr)easy);
            easy.SetOpt(CURLOPT.SHARE, (IntPtr)share);
            CurlMultiUpdater.Instance.AddMulti(this);
        }

        public void RemoveHandle(CurlEasy easy)
        {
            workingEasies.Remove((IntPtr)easy);
            Lib.curl_multi_remove_handle(multiPtr, (IntPtr)easy);

            if (workingEasies.Count == 0)
            {
                CurlMultiUpdater.Instance.RemoveMulti(this);
            }
        }

        internal void Tick()
        {
            long running = 0;
            Lib.curl_multi_perform(multiPtr, ref running);

            long index = 0;
            while (true)
            {
                var msgPtr = Lib.curl_multi_info_read(multiPtr, ref index);
                if (msgPtr != IntPtr.Zero)
                {
                    var msg = (CurlMsg)msgPtr;
                    if (msg.message == CURLMSG.DONE)
                    {
                        if (workingEasies.TryGetValue(msg.easyPtr, out var easy))
                        {
                            RemoveHandle(easy);
                            easy.OnMultiPerform(msg.result, this);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public static explicit operator IntPtr(CurlMulti multi)
        {
            return multi.multiPtr;
        }

        public static explicit operator CurlMulti(IntPtr ptr)
        {
            return new CurlMulti(ptr);
        }
    }
}