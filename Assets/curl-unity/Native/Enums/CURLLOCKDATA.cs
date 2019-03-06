namespace CurlUnity
{
    public enum CURLLOCKDATA
    {
        NONE = 0,
        /*  SHARE is used internally to say that
         *  the locking is just made to change the internal state of the share
         *  itself.
         */
        SHARE,
        COOKIE,
        DNS,
        SSL_SESSION,
        CONNECT,
        PSL,
        LAST,
    }
}