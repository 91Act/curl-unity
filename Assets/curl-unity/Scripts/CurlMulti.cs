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

        internal void CleanUp()
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
            Abort();
        }

        public void Abort()
        {
            if (multiPtr != IntPtr.Zero)
            {
                CurlEasy[] easies = null;

                lock (this)
                {
                    easies = workingEasies.Values.ToArray();
                }

                foreach (var easy in easies)
                {
                    easy.Abort();
                }

                CleanUp();
            }
        }

        internal void AddEasy(CurlEasy easy)
        {
            Lib.curl_multi_add_handle(multiPtr, (IntPtr)easy);
            easy.SetOpt(CURLOPT.SHARE, (IntPtr)share);

            int workingCount = 0;

            lock (this)
            {
                workingEasies[(IntPtr)easy] = easy;
                workingCount = workingEasies.Count;
            }
            if (workingCount == 1 && CurlMultiUpdater.Instance != null)
            {
                CurlMultiUpdater.Instance.AddMulti(this);
            }
        }

        internal void RemoveEasy(CurlEasy easy)
        {
            Lib.curl_multi_remove_handle(multiPtr, (IntPtr)easy);

            int workingCount = 0;

            lock (this)
            {
                workingEasies.Remove((IntPtr)easy);
                workingCount = workingEasies.Count;
            }

            if (workingCount == 0 && CurlMultiUpdater.Instance != null)
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
                            CurlEasy easy = null;

                            lock (this)
                            {
                                workingEasies.TryGetValue(msg.easyPtr, out easy);
                            }

                            if (easy != null)
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