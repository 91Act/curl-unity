namespace CurlUnity
{
    public static class HTTPcode
    {
        public enum Version
        {
            CURL_HTTP_VERSION_NONE, /* setting this means we don't care, and that we'd
                             like the library to choose the best possible
                             for us! */
            CURL_HTTP_VERSION_1_0,  /* please use HTTP 1.0 in the request */
            CURL_HTTP_VERSION_1_1,  /* please use HTTP 1.1 in the request */
            CURL_HTTP_VERSION_2_0,  /* please use HTTP 2 in the request */
            CURL_HTTP_VERSION_2TLS, /* use version 2 for HTTPS, version 1.1 for HTTP */
            CURL_HTTP_VERSION_2_PRIOR_KNOWLEDGE,  /* please use HTTP 2 without HTTP/1.1
                                           Upgrade */

            CURL_HTTP_VERSION_LAST /* *ILLEGAL* http version */
        }

        public enum SslVersion {
            CURL_SSLVERSION_DEFAULT,
            CURL_SSLVERSION_TLSv1, /* TLS 1.x */
            CURL_SSLVERSION_SSLv2,
            CURL_SSLVERSION_SSLv3,
            CURL_SSLVERSION_TLSv1_0,
            CURL_SSLVERSION_TLSv1_1,
            CURL_SSLVERSION_TLSv1_2,
            CURL_SSLVERSION_TLSv1_3,

            CURL_SSLVERSION_LAST, /* never use, keep last */

            CURL_SSLVERSION_MAX_NONE = 0,
            CURL_SSLVERSION_MAX_DEFAULT = (CURL_SSLVERSION_TLSv1 << 16),
            CURL_SSLVERSION_MAX_TLSv1_0 = (CURL_SSLVERSION_TLSv1_0 << 16),
            CURL_SSLVERSION_MAX_TLSv1_1 = (CURL_SSLVERSION_TLSv1_1 << 16),
            CURL_SSLVERSION_MAX_TLSv1_2 = (CURL_SSLVERSION_TLSv1_2 << 16),
            CURL_SSLVERSION_MAX_TLSv1_3 = (CURL_SSLVERSION_TLSv1_3 << 16),

            /* never use, keep last */
            CURL_SSLVERSION_MAX_LAST = (CURL_SSLVERSION_LAST << 16)
        };
    }
}