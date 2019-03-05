namespace CurlUnity
{
    public enum CURLM
    {
        CALL_MULTI_PERFORM = -1, /* please call curl_multi_perform() or
                                    curl_multi_socket*() soon */
        OK,
        BAD_HANDLE,      /* the passed-in handle is not a valid CURLM handle */
        BAD_EASY_HANDLE, /* an easy handle was not good/valid */
        OUT_OF_MEMORY,   /* if you ever get this, you're in deep sh*t */
        INTERNAL_ERROR,  /* this is a libcurl bug */
        BAD_SOCKET,      /* the passed in socket argument did not match */
        UNKNOWN_OPTION,  /* curl_multi_setopt() with unsupported option */
        ADDED_ALREADY,   /* an easy handle already added to a multi handle was
                            attempted to get added - again */
        RECURSIVE_API_CALL, /* an api function was called from inside a
                               callback */
        LAST
    }
}