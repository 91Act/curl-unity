namespace CurlUnity
{
    public enum HTTPVersion
    {
        VERSION_NONE, /* setting this means we don't care, and that we'd
                             like the library to choose the best possible
                             for us! */
        VERSION_1_0,  /* please use HTTP 1.0 in the request */
        VERSION_1_1,  /* please use HTTP 1.1 in the request */
        VERSION_2_0,  /* please use HTTP 2 in the request */
        VERSION_2TLS, /* use version 2 for HTTPS, version 1.1 for HTTP */
        VERSION_2_PRIOR_KNOWLEDGE,  /* please use HTTP 2 without HTTP/1.1
                                           Upgrade */
        VERSION_3 = 30, /* Makes use of explicit HTTP/3 without fallback.
                               Use CURLOPT_ALTSVC to enable HTTP/3 upgrade */

        VERSION_LAST /* *ILLEGAL* http version */
    }
}