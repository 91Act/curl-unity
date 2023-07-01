using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CurlUnity
{
    public class CurlShare : IDisposable
    {
        private IntPtr sharePtr;
        private Mutex mutex;
        private GCHandle thisHandle;

        public CurlShare(IntPtr ptr = default)
        {
            if (ptr != IntPtr.Zero)
            {
                sharePtr = ptr;
            }
            else
            {
                sharePtr = Lib.curl_share_init();
            }
        }

        public void CleanUp()
        {
            if (sharePtr != IntPtr.Zero)
            {
                Lib.curl_share_cleanup(sharePtr);
                sharePtr = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            CleanUp();
        }

        public CURLSH SetOpt(CURLSHOPT options, long value)
        {
            return Lib.curl_share_setopt_int(sharePtr, options, value);
        }

        public CURLSH SetOpt(CURLSHOPT options, bool value)
        {
            return Lib.curl_share_setopt_int(sharePtr, options, value ? 1 : 0);
        }

        public CURLSH SetOpt(CURLSHOPT options, string value)
        {
            return Lib.curl_share_setopt_str(sharePtr, options, value);
        }

        public CURLSH SetOpt(CURLSHOPT options, IntPtr value)
        {
            return Lib.curl_share_setopt_ptr(sharePtr, options, value);
        }

        public CURLSH SetOpt(CURLSHOPT options, byte[] value)
        {
            return Lib.curl_share_setopt_ptr(sharePtr, options, value);
        }

        public CURLSH SetOpt(CURLSHOPT options, Delegates.LockFunction value)
        {
            return Lib.curl_share_setopt_ptr(sharePtr, options, value);
        }

        public CURLSH SetOpt(CURLSHOPT options, Delegates.UnlockFunction value)
        {
            return Lib.curl_share_setopt_ptr(sharePtr, options, value);
        }

        public void SetupLock(bool on)
        {
            if (on)
            {
                mutex = new Mutex();
                thisHandle = GCHandle.Alloc(this);
                SetOpt(CURLSHOPT.USERDATA, (IntPtr)thisHandle);
                SetOpt(CURLSHOPT.LOCKFUNC, LockCallback);
                SetOpt(CURLSHOPT.UNLOCKFUNC, UnlockCallback);
            }
            else
            {
                mutex = null;
                if (thisHandle.IsAllocated) thisHandle.Free();
                SetOpt(CURLSHOPT.USERDATA, IntPtr.Zero);
                SetOpt(CURLSHOPT.LOCKFUNC, IntPtr.Zero);
                SetOpt(CURLSHOPT.UNLOCKFUNC, IntPtr.Zero);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(Delegates.LockFunction))]
        public static void LockCallback(IntPtr ptr, CURLLOCKDATA data, CURLLOCKACCESS access, IntPtr userdata)
        {
            var thiz = ((GCHandle)userdata).Target as CurlShare;
            thiz.mutex.WaitOne();
        }

        [AOT.MonoPInvokeCallback(typeof(Delegates.UnlockFunction))]
        public static void UnlockCallback(IntPtr ptr, CURLLOCKDATA data, IntPtr userdata)
        {
            var thiz = ((GCHandle)userdata).Target as CurlShare;
            thiz.mutex.ReleaseMutex();
        }

        public static explicit operator IntPtr(CurlShare share)
        {
            return share.sharePtr;
        }

        public static explicit operator CurlShare(IntPtr ptr)
        {
            return new CurlShare(ptr);
        }
    }
}
