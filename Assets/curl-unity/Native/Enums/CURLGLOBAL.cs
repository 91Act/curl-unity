using System;

namespace CurlUnity
{
    [Flags]
    public enum CURLGLOBAL
    {
        SSL = (1<<0), /* no purpose since since 7.57.0 */
        WIN32 = (1<<1),
        ALL = (SSL|WIN32),
        NOTHING = 0,
        DEFAULT = ALL,
        ACK_EINTR = (1<<2)
    }
}