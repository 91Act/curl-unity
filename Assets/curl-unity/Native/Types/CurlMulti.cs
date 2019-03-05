using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CurlUnity
{
    public class CurlMulti : IDisposable
    {
        private IntPtr multiPtr;

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
        }

        public void Dispose()
        {
            Lib.curl_multi_cleanup(multiPtr);
        }

        public void AddHandle(CurlEasy easy)
        {
            workingEasies[(IntPtr)easy] = easy;
            Lib.curl_multi_add_handle(multiPtr, (IntPtr)easy);
        }

        public void RemoveHandle(CurlEasy easy)
        {
            workingEasies.Remove((IntPtr)easy);
            Lib.curl_multi_remove_handle(multiPtr, (IntPtr)easy);
        }

        public CURLM Perform(ref long running)
        {
            return Lib.curl_multi_perform(multiPtr, ref running);
        }

        public void Tick()
        {
            long running = 0;
            Perform(ref running);

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
                            easy.OnPerformComplete(msg.result, this);
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