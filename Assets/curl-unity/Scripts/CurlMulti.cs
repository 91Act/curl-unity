using System;
using System.Collections.Generic;
using System.Linq;

namespace CurlUnity
{
    public class CurlMulti : IDisposable
    {
        public delegate void MultiPerformCallback(CURLE result, CurlMulti multi);
        
        private IntPtr multiPtr;
        private CurlShare share;
        private Dictionary<IntPtr, CurlEasy> workingEasies = new Dictionary<IntPtr, CurlEasy>();

        private static CurlMulti defaultInstance;

        public static CurlMulti DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new CurlMulti();
                }

                return defaultInstance;
            }
        }

        public CurlMulti(IntPtr ptr = default)
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

        public void CleanUp()
        {
            if (multiPtr != IntPtr.Zero)
            {
                CurlMultiUpdater.Instance.RemoveMulti(this);
                Lib.curl_multi_cleanup(multiPtr);
                multiPtr = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            CleanUp();
        }

        public void Abort()
        {
            foreach (var easy in workingEasies.Values.ToList())
            {
                easy.Abort();
                easy.CleanUp();
            }
        }

        public void AddEasy(CurlEasy easy)
        {
            workingEasies[(IntPtr)easy] = easy;
            Lib.curl_multi_add_handle(multiPtr, (IntPtr)easy);
            easy.SetOpt(CURLOPT.SHARE, (IntPtr)share);

            if (workingEasies.Count == 1)
            {
                CurlMultiUpdater.Instance.AddMulti(this);
            }
        }

        public void RemoveEasy(CurlEasy easy)
        {
            workingEasies.Remove((IntPtr)easy);
            Lib.curl_multi_remove_handle(multiPtr, (IntPtr)easy);

            if (workingEasies.Count == 0)
            {
                CurlMultiUpdater.Instance.RemoveMulti(this);
            }
        }

        internal int Perform()
        {
            long running = 0;

            if (multiPtr != IntPtr.Zero)
            {
                Lib.curl_multi_perform(multiPtr, ref running);

                while (true)
                {
                    long index = 0;
                    var msgPtr = Lib.curl_multi_info_read(multiPtr, ref index);
                    if (msgPtr != IntPtr.Zero)
                    {
                        var msg = (CurlMsg)msgPtr;
                        if (msg.message == CURLMSG.DONE)
                        {
                            if (workingEasies.TryGetValue(msg.easyPtr, out var easy))
                            {
                                RemoveEasy(easy);
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

            return (int)running;
        }

        internal void SetupLock(bool on)
        {
            share.SetupLock(on);
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