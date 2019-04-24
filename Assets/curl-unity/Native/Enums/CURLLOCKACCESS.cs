namespace CurlUnity
{
    public enum CURLLOCKACCESS
    {
        NONE = 0,   /* unspecified action */
        SHARED = 1, /* for read perhaps */
        SINGLE = 2, /* for write perhaps */
        LAST        /* never use */
    }
}