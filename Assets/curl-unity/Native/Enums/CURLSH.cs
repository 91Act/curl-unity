namespace CurlUnity
{
    public enum CURLSH
    {
        OK,  /* all is fine */
        BAD_OPTION, /* 1 */
        IN_USE,     /* 2 */
        INVALID,    /* 3 */
        NOMEM,      /* 4 out of memory */
        NOT_BUILT_IN, /* 5 feature not present in lib */
        LAST        /* never use */
    }
}