namespace CurlUnity
{
    public enum CURLMSG
    {
        NONE, /* first, not used */
        DONE, /* This easy handle has completed. 'result' contains
                   the CURLcode of the transfer */
        LAST /* last, not used */
    }
}