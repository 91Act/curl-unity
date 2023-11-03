namespace CurlUnity
{
    public enum CURLINFODEBUG
    {
        TEXT = 0,
        HEADER_IN,    /* 1 */
        HEADER_OUT,   /* 2 */
        DATA_IN,      /* 3 */
        DATA_OUT,     /* 4 */
        SSL_DATA_IN,  /* 5 */
        SSL_DATA_OUT, /* 6 */
        END
    }

    public enum CURLINFO
    {
        NONE, /* first, never use this */

        STRING = 0x100000,
        LONG = 0x200000,
        DOUBLE = 0x300000,
        SLIST = 0x400000,
        PTR = 0x400000, /* same as SLIST */
        SOCKET = 0x500000,
        OFF_T = 0x600000,
        MASK = 0x0fffff,
        TYPEMASK = 0xf00000,

        EFFECTIVE_URL = STRING + 1,
        RESPONSE_CODE = LONG + 2,
        TOTAL_TIME = DOUBLE + 3,
        NAMELOOKUP_TIME = DOUBLE + 4,
        CONNECT_TIME = DOUBLE + 5,
        PRETRANSFER_TIME = DOUBLE + 6,
        SIZE_UPLOAD = DOUBLE + 7,
        SIZE_UPLOAD_T = OFF_T + 7,
        SIZE_DOWNLOAD = DOUBLE + 8,
        SIZE_DOWNLOAD_T = OFF_T + 8,
        SPEED_DOWNLOAD = DOUBLE + 9,
        SPEED_DOWNLOAD_T = OFF_T + 9,
        SPEED_UPLOAD = DOUBLE + 10,
        SPEED_UPLOAD_T = OFF_T + 10,
        HEADER_SIZE = LONG + 11,
        REQUEST_SIZE = LONG + 12,
        SSL_VERIFYRESULT = LONG + 13,
        FILETIME = LONG + 14,
        FILETIME_T = OFF_T + 14,
        CONTENT_LENGTH_DOWNLOAD = DOUBLE + 15,
        CONTENT_LENGTH_DOWNLOAD_T = OFF_T + 15,
        CONTENT_LENGTH_UPLOAD = DOUBLE + 16,
        CONTENT_LENGTH_UPLOAD_T = OFF_T + 16,
        STARTTRANSFER_TIME = DOUBLE + 17,
        CONTENT_TYPE = STRING + 18,
        REDIRECT_TIME = DOUBLE + 19,
        REDIRECT_COUNT = LONG + 20,
        PRIVATE = STRING + 21,
        HTTP_CONNECTCODE = LONG + 22,
        HTTPAUTH_AVAIL = LONG + 23,
        PROXYAUTH_AVAIL = LONG + 24,
        OS_ERRNO = LONG + 25,
        NUM_CONNECTS = LONG + 26,
        SSL_ENGINES = SLIST + 27,
        COOKIELIST = SLIST + 28,
        LASTSOCKET = LONG + 29,
        FTP_ENTRY_PATH = STRING + 30,
        REDIRECT_URL = STRING + 31,
        PRIMARY_IP = STRING + 32,
        APPCONNECT_TIME = DOUBLE + 33,
        CERTINFO = PTR + 34,
        CONDITION_UNMET = LONG + 35,
        RTSP_SESSION_ID = STRING + 36,
        RTSP_CLIENT_CSEQ = LONG + 37,
        RTSP_SERVER_CSEQ = LONG + 38,
        RTSP_CSEQ_RECV = LONG + 39,
        PRIMARY_PORT = LONG + 40,
        LOCAL_IP = STRING + 41,
        LOCAL_PORT = LONG + 42,
        TLS_SESSION = PTR + 43,
        ACTIVESOCKET = SOCKET + 44,
        TLS_SSL_PTR = PTR + 45,
        HTTP_VERSION = LONG + 46,
        PROXY_SSL_VERIFYRESULT = LONG + 47,
        PROTOCOL = LONG + 48,
        SCHEME = STRING + 49,
        TOTAL_TIME_T = OFF_T + 50,
        NAMELOOKUP_TIME_T = OFF_T + 51,
        CONNECT_TIME_T = OFF_T + 52,
        PRETRANSFER_TIME_T = OFF_T + 53,
        STARTTRANSFER_TIME_T = OFF_T + 54,
        REDIRECT_TIME_T = OFF_T + 55,
        APPCONNECT_TIME_T = OFF_T + 56,
        RETRY_AFTER = OFF_T + 57,
        EFFECTIVE_METHOD = STRING + 58,
        PROXY_ERROR = LONG + 59,
        REFERER = STRING + 60,
        CAINFO = STRING + 61,
        CAPATH = STRING + 62,
        LASTONE = 62
    }
}
