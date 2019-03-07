using System;

namespace CurlUnity
{
    public class CurlShare : IDisposable
    {
        private IntPtr sharePtr;

        public CurlShare(IntPtr ptr = default(IntPtr))
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
            return Lib.curl_share_setopt_int(sharePtr, options, value);
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