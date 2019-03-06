namespace CurlUnity
{
    public enum CURLSHOPT
    {
        NONE,  /* don't use */
        SHARE,   /* specify a data type to share */
        UNSHARE, /* specify which data type to stop sharing */
        LOCKFUNC,   /* pass in a 'curl_lock_function' pointer */
        UNLOCKFUNC, /* pass in a 'curl_unlock_function' pointer */
        USERDATA,   /* pass in a user data pointer used in the lock/unlock
                           callback functions */
        LAST  /* never use */
    }
}