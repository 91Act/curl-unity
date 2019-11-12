namespace CurlUnity
{
    public enum CURLMOPT
    {
        /* This is the socket callback function pointer */
        SOCKETFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 1,

        /* This is the argument passed to the socket callback */
        SOCKETDATA = CURLOPTTYPE.OBJECTPOINT + 2,

        /* set to 1 to enable pipelining for this multi handle */
        PIPELINING = CURLOPTTYPE.LONG + 3,

        /* This is the timer callback function pointer */
        TIMERFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 4,

        /* This is the argument passed to the timer callback */
        TIMERDATA = CURLOPTTYPE.OBJECTPOINT + 5,

        /* maximum number of entries in the connection cache */
        MAXCONNECTS = CURLOPTTYPE.LONG + 6,

        /* maximum number of (pipelining) connections to one host */
        MAX_HOST_CONNECTIONS = CURLOPTTYPE.LONG + 7,

        /* maximum number of requests in a pipeline */
        MAX_PIPELINE_LENGTH = CURLOPTTYPE.LONG + 8,

        /* a connection with a content-length longer than this
           will not be considered for pipelining */
        CONTENT_LENGTH_PENALTY_SIZE = CURLOPTTYPE.OFF_T + 9,

        /* a connection with a chunk length longer than this
           will not be considered for pipelining */
        CHUNK_LENGTH_PENALTY_SIZE = CURLOPTTYPE.OFF_T + 10,

        /* a list of site names(+port) that are blacklisted from
           pipelining */
        PIPELINING_SITE_BL = CURLOPTTYPE.OBJECTPOINT + 11,

        /* a list of server types that are blacklisted from
           pipelining */
        PIPELINING_SERVER_BL = CURLOPTTYPE.OBJECTPOINT + 12,

        /* maximum number of open connections in total */
        MAX_TOTAL_CONNECTIONS = CURLOPTTYPE.LONG + 13,

        /* This is the server push callback function pointer */
        PUSHFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 14,

        /* This is the argument passed to the server push callback */
        PUSHDATA = CURLOPTTYPE.OBJECTPOINT + 15,

        /* maximum number of concurrent streams to support on a connection */
        MAX_CONCURRENT_STREAMS = CURLOPTTYPE.LONG + 16,

        CURLMOPT_LASTENTRY /* the last unused */
    }
}