namespace CurlUnity
{

    public enum SslVersion
    {
        DEFAULT,
        TLSv1, /* TLS 1.x */
        SSLv2,
        SSLv3,
        TLSv1_0,
        TLSv1_1,
        TLSv1_2,
        TLSv1_3,

        LAST, /* never use, keep last */

        MAX_NONE = 0,
        MAX_DEFAULT = (TLSv1 << 16),
        MAX_TLSv1_0 = (TLSv1_0 << 16),
        MAX_TLSv1_1 = (TLSv1_1 << 16),
        MAX_TLSv1_2 = (TLSv1_2 << 16),
        MAX_TLSv1_3 = (TLSv1_3 << 16),

        /* never use, keep last */
        MAX_LAST = (LAST << 16)
    }
}
