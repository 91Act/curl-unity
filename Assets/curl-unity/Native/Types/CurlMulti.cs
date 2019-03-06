using System;
using System.Collections.Generic;

namespace CurlUnity
{
    public class CurlMulti : IDisposable
    {
        public delegate void MultiPerformCallback(CURLE result, CurlMulti multi);

        private IntPtr multiPtr;

        private Dictionary<IntPtr, MultiPerformCallback> workingEasies = new Dictionary<IntPtr, MultiPerformCallback>();

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
        }

        public void Dispose()
        {
            Lib.curl_multi_cleanup(multiPtr);
        }

        public void AddHandle(CurlEasy easy, MultiPerformCallback callback)
        {
            workingEasies[(IntPtr)easy] = callback;
            Lib.curl_multi_add_handle(multiPtr, (IntPtr)easy);
            CurlMultiUpdater.Instance.AddMulti(this);
        }

        public void RemoveHandle(IntPtr easyPtr)
        {
            workingEasies.Remove(easyPtr);
            Lib.curl_multi_remove_handle(multiPtr, easyPtr);

            if (workingEasies.Count == 0)
            {
                CurlMultiUpdater.Instance.RemoveMulti(this);
            }
        }

        public void RemoveHandle(CurlEasy easy)
        {
            RemoveHandle((IntPtr)easy);
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
                        if (workingEasies.TryGetValue(msg.easyPtr, out var callback))
                        {
                            RemoveHandle(msg.easyPtr);
                            callback(msg.result, this);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}