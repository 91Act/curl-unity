namespace CurlUnity
{
    public static class CURLOPTTYPE
    {
        public const int LONG = 0;
        public const int OBJECTPOINT = 10000;
        public const int FUNCTIONPOINT = 20000;
        public const int OFF_T = 30000;
        public const int BLOB = 40000;
        public const int STRINGPOINT = OBJECTPOINT;
        public const int SLISTPOINT = OBJECTPOINT;
        public const int CBPOINT = OBJECTPOINT;
        public const int VALUES = LONG;
    }

    public enum CURLOPT
    {
        /* This is the FILE * or void * the regular output should be written to. */
        WRITEDATA = CURLOPTTYPE.OBJECTPOINT + 1,

        /* The full URL to get/put */
        URL = CURLOPTTYPE.STRINGPOINT + 2,

        /* Port number to connect to, if other than default. */
        PORT = CURLOPTTYPE.LONG + 3,

        /* Name of proxy to use. */
        PROXY = CURLOPTTYPE.STRINGPOINT + 4,

        /* "user:password;options" to use when fetching. */
        USERPWD = CURLOPTTYPE.STRINGPOINT + 5,

        /* "user:password" to use with proxy. */
        PROXYUSERPWD = CURLOPTTYPE.STRINGPOINT + 6,

        /* Range to get, specified as an ASCII string. */
        RANGE = CURLOPTTYPE.STRINGPOINT + 7,

        /* not used */

        /* Specified file stream to upload from (use as input): */
        READDATA = CURLOPTTYPE.OBJECTPOINT + 9,

        /* Buffer to receive error messages in, must be at least CURL_ERROR_SIZE
         * bytes big. */
        ERRORBUFFER = CURLOPTTYPE.OBJECTPOINT + 10,

        /* Function that will be called to store the output (instead of fwrite). The
         * parameters will use fwrite() syntax, make sure to follow them. */
        WRITEFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 11,

        /* Function that will be called to read the input (instead of fread). The
         * parameters will use fread() syntax, make sure to follow them. */
        READFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 12,

        /* Time-out the read operation after this amount of seconds */
        TIMEOUT = CURLOPTTYPE.LONG + 13,

        /* If the INFILE is used, this can be used to inform libcurl about
         * how large the file being sent really is. That allows better error
         * checking and better verifies that the upload was successful. -1 means
         * unknown size.
         *
         * For large file support, there is also a _LARGE version of the key
         * which takes an off_t type, allowing platforms with larger off_t
         * sizes to handle larger files.  See below for INFILESIZE_LARGE.
         */
        INFILESIZE = CURLOPTTYPE.LONG + 14,

        /* POST static input fields. */
        POSTFIELDS = CURLOPTTYPE.OBJECTPOINT + 15,

        /* Set the referrer page (needed by some CGIs) */
        REFERER = CURLOPTTYPE.STRINGPOINT + 16,

        /* Set the FTP PORT string (interface name, named or numerical IP address)
           Use i.e '-' to use default address. */
        FTPPORT = CURLOPTTYPE.STRINGPOINT + 17,

        /* Set the User-Agent string (examined by some CGIs) */
        USERAGENT = CURLOPTTYPE.STRINGPOINT + 18,

        /* If the download receives less than "low speed limit" bytes/second
         * during "low speed time" seconds, the operations is aborted.
         * You could i.e if you have a pretty high speed connection, abort if
         * it is less than 2000 bytes/sec during 20 seconds.
         */

        /* Set the "low speed limit" */
        LOW_SPEED_LIMIT = CURLOPTTYPE.LONG + 19,

        /* Set the "low speed time" */
        LOW_SPEED_TIME = CURLOPTTYPE.LONG + 20,

        /* Set the continuation offset.
         *
         * Note there is also a _LARGE version of this key which uses
         * off_t types, allowing for large file offsets on platforms which
         * use larger-than-32-bit off_t's.  Look below for RESUME_FROM_LARGE.
         */
        RESUME_FROM = CURLOPTTYPE.LONG + 21,

        /* Set cookie in request: */
        COOKIE = CURLOPTTYPE.STRINGPOINT + 22,

        /* This points to a linked list of headers, struct curl_slist kind. This
           list is also used for RTSP (in spite of its name) */
        HTTPHEADER = CURLOPTTYPE.SLISTPOINT + 23,

        /* This points to a linked list of post entries, struct curl_httppost */
        HTTPPOST = CURLOPTTYPE.OBJECTPOINT + 24,

        /* name of the file keeping your private SSL-certificate */
        SSLCERT = CURLOPTTYPE.STRINGPOINT + 25,

        /* password for the SSL or SSH private key */
        KEYPASSWD = CURLOPTTYPE.STRINGPOINT + 26,

        /* send TYPE parameter? */
        CRLF = CURLOPTTYPE.LONG + 27,

        /* send linked-list of QUOTE commands */
        QUOTE = CURLOPTTYPE.SLISTPOINT + 28,

        /* send FILE * or void * to store headers to, if you use a callback it
           is simply passed to the callback unmodified */
        HEADERDATA = CURLOPTTYPE.OBJECTPOINT + 29,

        /* point to a file to read the initial cookies from, also enables
           "cookie awareness" */
        COOKIEFILE = CURLOPTTYPE.STRINGPOINT + 31,

        /* What version to specifically try to use.
           See CURL_SSLVERSION defines below. */
        SSLVERSION = CURLOPTTYPE.LONG + 32,

        /* What kind of HTTP time condition to use, see defines */
        TIMECONDITION = CURLOPTTYPE.LONG + 33,

        /* Time to use with the above condition. Specified in number of seconds
           since 1 Jan 1970 */
        TIMEVALUE = CURLOPTTYPE.LONG + 34,

        /* 35 = OBSOLETE */

        /* Custom request, for customizing the get command like
           HTTP: DELETE, TRACE and others
           FTP: to use a different list command
           */
        CUSTOMREQUEST = CURLOPTTYPE.STRINGPOINT + 36,

        /* FILE handle to use instead of stderr */
        STDERR = CURLOPTTYPE.OBJECTPOINT + 37,

        /* 38 is not used */

        /* send linked-list of post-transfer QUOTE commands */
        POSTQUOTE = CURLOPTTYPE.SLISTPOINT + 39,

        OBSOLETE40 = CURLOPTTYPE.OBJECTPOINT + 40, /* OBSOLETE, do not use! */

        VERBOSE = CURLOPTTYPE.LONG + 41,      /* talk a lot */
        HEADER = CURLOPTTYPE.LONG + 42,       /* throw the header out too */
        NOPROGRESS = CURLOPTTYPE.LONG + 43,   /* shut off the progress meter */
        NOBODY = CURLOPTTYPE.LONG + 44,       /* use HEAD to get http document */
        FAILONERROR = CURLOPTTYPE.LONG + 45,  /* no output on http error codes >= 400 */
        UPLOAD = CURLOPTTYPE.LONG + 46,       /* this is an upload */
        POST = CURLOPTTYPE.LONG + 47,         /* HTTP POST method */
        DIRLISTONLY = CURLOPTTYPE.LONG + 48,  /* bare names when listing directories */

        APPEND = CURLOPTTYPE.LONG + 50,       /* Append instead of overwrite on upload! */

        /* Specify whether to read the user+password from the .netrc or the URL.
         * This must be one of the CURL_NETRC_* enums below. */
        NETRC = CURLOPTTYPE.LONG + 51,

        FOLLOWLOCATION = CURLOPTTYPE.LONG + 52,  /* use Location: Luke! */

        TRANSFERTEXT = CURLOPTTYPE.LONG + 53, /* transfer data in text/ASCII format */
        PUT = CURLOPTTYPE.LONG + 54,          /* HTTP PUT */

        /* 55 = OBSOLETE */

        /* DEPRECATED
         * Function that will be called instead of the internal progress display
         * function. This function should be defined as the curl_progress_callback
         * prototype defines. */
        PROGRESSFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 56,

        /* Data passed to the PROGRESSFUNCTION and XFERINFOFUNCTION
           callbacks */
        PROGRESSDATA = CURLOPTTYPE.OBJECTPOINT + 57,
        XFERINFODATA = PROGRESSDATA,

        /* We want the referrer field set automatically when following locations */
        AUTOREFERER = CURLOPTTYPE.LONG + 58,

        /* Port of the proxy, can be set in the proxy string as well with:
           "[host]:[port]" */
        PROXYPORT = CURLOPTTYPE.LONG + 59,

        /* size of the POST input data, if strlen() is not good to use */
        POSTFIELDSIZE = CURLOPTTYPE.LONG + 60,

        /* tunnel non-http operations through a HTTP proxy */
        HTTPPROXYTUNNEL = CURLOPTTYPE.LONG + 61,

        /* Set the interface string to use as outgoing network interface */
        INTERFACE = CURLOPTTYPE.STRINGPOINT + 62,

        /* Set the krb4/5 security level, this also enables krb4/5 awareness.  This
         * is a string, 'clear', 'safe', 'confidential' or 'private'.  If the string
         * is set but doesn't match one of these, 'private' will be used.  */
        KRBLEVEL = CURLOPTTYPE.STRINGPOINT + 63,

        /* Set if we should verify the peer in ssl handshake, set 1 to verify. */
        SSL_VERIFYPEER = CURLOPTTYPE.LONG + 64,

        /* The CApath or CAfile used to validate the peer certificate
           this option is used only if SSL_VERIFYPEER is true */
        CAINFO = CURLOPTTYPE.STRINGPOINT + 65,

        /* 66 = OBSOLETE */
        /* 67 = OBSOLETE */

        /* Maximum number of http redirects to follow */
        MAXREDIRS = CURLOPTTYPE.LONG + 68,

        /* Pass a long set to 1 to get the date of the requested document (if
           possible)! Pass a zero to shut it off. */
        FILETIME = CURLOPTTYPE.LONG + 69,

        /* This points to a linked list of telnet options */
        TELNETOPTIONS = CURLOPTTYPE.SLISTPOINT + 70,

        /* Max amount of cached alive connections */
        MAXCONNECTS = CURLOPTTYPE.LONG + 71,

        OBSOLETE72 = CURLOPTTYPE.LONG + 72, /* OBSOLETE, do not use! ,

        /* 73 = OBSOLETE */

        /* Set to explicitly use a new connection for the upcoming transfer.
           Do not use this unless you're absolutely sure of this, as it makes the
           operation slower and is less friendly for the network. */
        FRESH_CONNECT = CURLOPTTYPE.LONG + 74,

        /* Set to explicitly forbid the upcoming transfer's connection to be re-used
           when done. Do not use this unless you're absolutely sure of this, as it
           makes the operation slower and is less friendly for the network. */
        FORBID_REUSE = CURLOPTTYPE.LONG + 75,

        /* Set to a file name that contains random data for libcurl to use to
           seed the random engine when doing SSL connects. */
        RANDOM_FILE = CURLOPTTYPE.STRINGPOINT + 76,

        /* Set to the Entropy Gathering Daemon socket pathname */
        EGDSOCKET = CURLOPTTYPE.STRINGPOINT + 77,

        /* Time-out connect operations after this amount of seconds, if connects are
           OK within this time, then fine... This only aborts the connect phase. */
        CONNECTTIMEOUT = CURLOPTTYPE.LONG + 78,

        /* Function that will be called to store headers (instead of fwrite). The
         * parameters will use fwrite() syntax, make sure to follow them. */
        HEADERFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 79,

        /* Set this to force the HTTP request to get back to GET. Only really usable
           if POST, PUT or a custom request have been used first.
         */
        HTTPGET = CURLOPTTYPE.LONG + 80,

        /* Set if we should verify the Common name from the peer certificate in ssl
         * handshake, set 1 to check existence, 2 to ensure that it matches the
         * provided hostname. */
        SSL_VERIFYHOST = CURLOPTTYPE.LONG + 81,

        /* Specify which file name to write all known cookies in after completed
           operation. Set file name to "-" (dash) to make it go to stdout. */
        COOKIEJAR = CURLOPTTYPE.STRINGPOINT + 82,

        /* Specify which SSL ciphers to use */
        SSL_CIPHER_LIST = CURLOPTTYPE.STRINGPOINT + 83,

        /* Specify which HTTP version to use! This must be set to one of the
           CURL_HTTP_VERSION* enums set below. */
        HTTP_VERSION = CURLOPTTYPE.LONG + 84,

        /* Specifically switch on or off the FTP engine's use of the EPSV command. By
           default, that one will always be attempted before the more traditional
           PASV command. */
        FTP_USE_EPSV = CURLOPTTYPE.LONG + 85,

        /* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") */
        SSLCERTTYPE = CURLOPTTYPE.STRINGPOINT + 86,

        /* name of the file keeping your private SSL-key */
        SSLKEY = CURLOPTTYPE.STRINGPOINT + 87,

        /* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") */
        SSLKEYTYPE = CURLOPTTYPE.STRINGPOINT + 88,

        /* crypto engine for the SSL-sub system */
        SSLENGINE = CURLOPTTYPE.STRINGPOINT + 89,

        /* set the crypto engine for the SSL-sub system as default
           the param has no meaning...
         */
        SSLENGINE_DEFAULT = CURLOPTTYPE.LONG + 90,

        /* Non-zero value means to use the global dns cache */
        DNS_USE_GLOBAL_CACHE = CURLOPTTYPE.LONG + 91, /* DEPRECATED, do not use! ,

        /* DNS cache timeout */
        DNS_CACHE_TIMEOUT = CURLOPTTYPE.LONG + 92,

        /* send linked-list of pre-transfer QUOTE commands */
        PREQUOTE = CURLOPTTYPE.SLISTPOINT + 93,

        /* set the debug function */
        DEBUGFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 94,

        /* set the data for the debug function */
        DEBUGDATA = CURLOPTTYPE.OBJECTPOINT + 95,

        /* mark this as start of a cookie session */
        COOKIESESSION = CURLOPTTYPE.LONG + 96,

        /* The CApath directory used to validate the peer certificate
           this option is used only if SSL_VERIFYPEER is true */
        CAPATH = CURLOPTTYPE.STRINGPOINT + 97,

        /* Instruct libcurl to use a smaller receive buffer */
        BUFFERSIZE = CURLOPTTYPE.LONG + 98,

        /* Instruct libcurl to not use any signal/alarm handlers, even when using
           timeouts. This option is useful for multi-threaded applications.
           See libcurl-the-guide for more background information. */
        NOSIGNAL = CURLOPTTYPE.LONG + 99,

        /* Provide a CURLShare for mutexing non-ts data */
        SHARE = CURLOPTTYPE.OBJECTPOINT + 100,

        /* indicates type of proxy. accepted values are CURLPROXY_HTTP (default),
           CURLPROXY_HTTPS, CURLPROXY_SOCKS4, CURLPROXY_SOCKS4A and
           CURLPROXY_SOCKS5. */
        PROXYTYPE = CURLOPTTYPE.LONG + 101,

        /* Set the Accept-Encoding string. Use this to tell a server you would like
           the response to be compressed. Before 7.21.6, this was known as
           ENCODING */
        ACCEPT_ENCODING = CURLOPTTYPE.STRINGPOINT + 102,

        /* Set pointer to private data */
        PRIVATE = CURLOPTTYPE.OBJECTPOINT + 103,

        /* Set aliases for HTTP 200 in the HTTP Response header */
        HTTP200ALIASES = CURLOPTTYPE.SLISTPOINT + 104,

        /* Continue to send authentication (user+password) when following locations,
           even when hostname changed. This can potentially send off the name
           and password to whatever host the server decides. */
        UNRESTRICTED_AUTH = CURLOPTTYPE.LONG + 105,

        /* Specifically switch on or off the FTP engine's use of the EPRT command (
           it also disables the LPRT attempt). By default, those ones will always be
           attempted before the good old traditional PORT command. */
        FTP_USE_EPRT = CURLOPTTYPE.LONG + 106,

        /* Set this to a bitmask value to enable the particular authentications
           methods you like. Use this in combination with USERPWD.
           Note that setting multiple bits may cause extra network round-trips. */
        HTTPAUTH = CURLOPTTYPE.LONG + 107,

        /* Set the ssl context callback function, currently only for OpenSSL or
           WolfSSL ssl_ctx, or mbedTLS mbedtls_ssl_config in the second argument.
           The function must match the curl_ssl_ctx_callback prototype. */
        SSL_CTX_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 108,

        /* Set the userdata for the ssl context callback function's third
           argument */
        SSL_CTX_DATA = CURLOPTTYPE.OBJECTPOINT + 109,

        /* FTP Option that causes missing dirs to be created on the remote server.
           In 7.19.4 we introduced the convenience enums for this option using the
           CURLFTP_CREATE_DIR prefix.
        */
        FTP_CREATE_MISSING_DIRS = CURLOPTTYPE.LONG + 110,

        /* Set this to a bitmask value to enable the particular authentications
           methods you like. Use this in combination with PROXYUSERPWD.
           Note that setting multiple bits may cause extra network round-trips. */
        PROXYAUTH = CURLOPTTYPE.LONG + 111,

        /* FTP option that changes the timeout, in seconds, associated with
           getting a response.  This is different from transfer timeout time and
           essentially places a demand on the FTP server to acknowledge commands
           in a timely manner. */
        FTP_RESPONSE_TIMEOUT = CURLOPTTYPE.LONG + 112,
        SERVER_RESPONSE_TIMEOUT = FTP_RESPONSE_TIMEOUT,

        /* Set this option to one of the CURL_IPRESOLVE_* defines (see below) to
           tell libcurl to resolve names to those IP versions only. This only has
           affect on systems with support for more than one, i.e IPv4 _and_ IPv6. */
        IPRESOLVE = CURLOPTTYPE.LONG + 113,

        /* Set this option to limit the size of a file that will be downloaded from
           an HTTP or FTP server.

           Note there is also _LARGE version which adds large file support for
           platforms which have larger off_t sizes.  See MAXFILESIZE_LARGE below. */
        MAXFILESIZE = CURLOPTTYPE.LONG + 114,

        /* See the comment for INFILESIZE above, but in short, specifies
         * the size of the file being uploaded.  -1 means unknown.
         */
        INFILESIZE_LARGE = CURLOPTTYPE.OFF_T + 115,

        /* Sets the continuation offset.  There is also a LONG version of this;
         * look above for RESUME_FROM.
         */
        RESUME_FROM_LARGE = CURLOPTTYPE.OFF_T + 116,

        /* Sets the maximum size of data that will be downloaded from
         * an HTTP or FTP server.  See MAXFILESIZE above for the LONG version.
         */
        MAXFILESIZE_LARGE = CURLOPTTYPE.OFF_T + 117,

        /* Set this option to the file name of your .netrc file you want libcurl
           to parse (using the NETRC option). If not set, libcurl will do
           a poor attempt to find the user's home directory and check for a .netrc
           file in there. */
        NETRC_FILE = CURLOPTTYPE.STRINGPOINT + 118,

        /* Enable SSL/TLS for FTP, pick one of:
           CURLUSESSL_TRY     - try using SSL, proceed anyway otherwise
           CURLUSESSL_CONTROL - SSL for the control connection or fail
           CURLUSESSL_ALL     - SSL for all communication or fail
        */
        USE_SSL = CURLOPTTYPE.LONG + 119,

        /* The _LARGE version of the standard POSTFIELDSIZE option */
        POSTFIELDSIZE_LARGE = CURLOPTTYPE.OFF_T + 120,

        /* Enable/disable the TCP Nagle algorithm */
        TCP_NODELAY = CURLOPTTYPE.LONG + 121,

        /* 122 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 123 OBSOLETE. Gone in 7.16.0 */
        /* 124 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 125 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 126 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 127 OBSOLETE. Gone in 7.16.0 */
        /* 128 OBSOLETE. Gone in 7.16.0 */

        /* When FTP over SSL/TLS is selected (with USE_SSL), this option
           can be used to change libcurl's default action which is to first try
           "AUTH SSL" and then "AUTH TLS" in this order, and proceed when a OK
           response has been received.

           Available parameters are:
           CURLFTPAUTH_DEFAULT - let libcurl decide
           CURLFTPAUTH_SSL     - try "AUTH SSL" first, then TLS
           CURLFTPAUTH_TLS     - try "AUTH TLS" first, then SSL
        */
        FTPSSLAUTH = CURLOPTTYPE.LONG + 129,

        IOCTLFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 130,
        IOCTLDATA = CURLOPTTYPE.OBJECTPOINT + 131,

        /* 132 OBSOLETE. Gone in 7.16.0 */
        /* 133 OBSOLETE. Gone in 7.16.0 */

        /* zero terminated string for pass on to the FTP server when asked for
           "account" info */
        FTP_ACCOUNT = CURLOPTTYPE.STRINGPOINT + 134,

        /* feed cookie into cookie engine */
        COOKIELIST = CURLOPTTYPE.STRINGPOINT + 135,

        /* ignore Content-Length */
        IGNORE_CONTENT_LENGTH = CURLOPTTYPE.LONG + 136,

        /* Set to non-zero to skip the IP address received in a 227 PASV FTP server
           response. Typically used for FTP-SSL purposes but is not restricted to
           that. libcurl will then instead use the same IP address it used for the
           control connection. */
        FTP_SKIP_PASV_IP = CURLOPTTYPE.LONG + 137,

        /* Select "file method" to use when doing FTP, see the curl_ftpmethod
           above. */
        FTP_FILEMETHOD = CURLOPTTYPE.LONG + 138,

        /* Local port number to bind the socket to */
        LOCALPORT = CURLOPTTYPE.LONG + 139,

        /* Number of ports to try, including the first one set with LOCALPORT.
           Thus, setting it to 1 will make no additional attempts but the first.
        */
        LOCALPORTRANGE = CURLOPTTYPE.LONG + 140,

        /* no transfer, set up connection and let application use the socket by
           extracting it with CURLINFO_LASTSOCKET */
        CONNECT_ONLY = CURLOPTTYPE.LONG + 141,

        /* Function that will be called to convert from the
           network encoding (instead of using the iconv calls in libcurl) */
        CONV_FROM_NETWORK_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 142,

        /* Function that will be called to convert to the
           network encoding (instead of using the iconv calls in libcurl) */
        CONV_TO_NETWORK_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 143,

        /* Function that will be called to convert from UTF8
           (instead of using the iconv calls in libcurl)
           Note that this is used only for SSL certificate processing */
        CONV_FROM_UTF8_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 144,

        /* if the connection proceeds too quickly then need to slow it down */
        /* limit-rate: maximum number of bytes per second to send or receive */
        MAX_SEND_SPEED_LARGE = CURLOPTTYPE.OFF_T + 145,
        MAX_RECV_SPEED_LARGE = CURLOPTTYPE.OFF_T + 146,

        /* Pointer to command string to send if USER/PASS fails. */
        FTP_ALTERNATIVE_TO_USER = CURLOPTTYPE.STRINGPOINT + 147,

        /* callback function for setting socket options */
        SOCKOPTFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 148,
        SOCKOPTDATA = CURLOPTTYPE.OBJECTPOINT + 149,

        /* set to 0 to disable session ID re-use for this transfer, default is
           enabled (== 1) */
        SSL_SESSIONID_CACHE = CURLOPTTYPE.LONG + 150,

        /* allowed SSH authentication methods */
        SSH_AUTH_TYPES = CURLOPTTYPE.LONG + 151,

        /* Used by scp/sftp to do public/private key authentication */
        SSH_PUBLIC_KEYFILE = CURLOPTTYPE.STRINGPOINT + 152,
        SSH_PRIVATE_KEYFILE = CURLOPTTYPE.STRINGPOINT + 153,

        /* Send CCC (Clear Command Channel) after authentication */
        FTP_SSL_CCC = CURLOPTTYPE.LONG + 154,

        /* Same as TIMEOUT and CONNECTTIMEOUT, but with ms resolution */
        TIMEOUT_MS = CURLOPTTYPE.LONG + 155,
        CONNECTTIMEOUT_MS = CURLOPTTYPE.LONG + 156,

        /* set to zero to disable the libcurl's decoding and thus pass the raw body
           data to the application even when it is encoded/compressed */
        HTTP_TRANSFER_DECODING = CURLOPTTYPE.LONG + 157,
        HTTP_CONTENT_DECODING = CURLOPTTYPE.LONG + 158,

        /* Permission used when creating new files and directories on the remote
           server for protocols that support it, SFTP/SCP/FILE */
        NEW_FILE_PERMS = CURLOPTTYPE.LONG + 159,
        NEW_DIRECTORY_PERMS = CURLOPTTYPE.LONG + 160,

        /* Set the behaviour of POST when redirecting. Values must be set to one
           of CURL_REDIR* defines below. This used to be called POST301 */
        POSTREDIR = CURLOPTTYPE.LONG + 161,

        /* used by scp/sftp to verify the host's public key */
        SSH_HOST_PUBLIC_KEY_MD5 = CURLOPTTYPE.STRINGPOINT + 162,

        /* Callback function for opening socket (instead of socket(2)). Optionally,
           callback is able change the address or refuse to connect returning
           CURL_SOCKET_BAD.  The callback should have type
           curl_opensocket_callback */
        OPENSOCKETFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 163,
        OPENSOCKETDATA = CURLOPTTYPE.OBJECTPOINT + 164,

        /* POST volatile input fields. */
        COPYPOSTFIELDS = CURLOPTTYPE.OBJECTPOINT + 165,

        /* set transfer mode (;type=<a|i>) when doing FTP via an HTTP proxy */
        PROXY_TRANSFER_MODE = CURLOPTTYPE.LONG + 166,

        /* Callback function for seeking in the input stream */
        SEEKFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 167,
        SEEKDATA = CURLOPTTYPE.OBJECTPOINT + 168,

        /* CRL file */
        CRLFILE = CURLOPTTYPE.STRINGPOINT + 169,

        /* Issuer certificate */
        ISSUERCERT = CURLOPTTYPE.STRINGPOINT + 170,

        /* (IPv6) Address scope */
        ADDRESS_SCOPE = CURLOPTTYPE.LONG + 171,

        /* Collect certificate chain info and allow it to get retrievable with
           CURLINFO_CERTINFO after the transfer is complete. */
        CERTINFO = CURLOPTTYPE.LONG + 172,

        /* "name" and "pwd" to use when fetching. */
        USERNAME = CURLOPTTYPE.STRINGPOINT + 173,
        PASSWORD = CURLOPTTYPE.STRINGPOINT + 174,

        /* "name" and "pwd" to use with Proxy when fetching. */
        PROXYUSERNAME = CURLOPTTYPE.STRINGPOINT + 175,
        PROXYPASSWORD = CURLOPTTYPE.STRINGPOINT + 176,

        /* Comma separated list of hostnames defining no-proxy zones. These should
           match both hostnames directly, and hostnames within a domain. For
           example, local.com will match local.com and www.local.com, but NOT
           notlocal.com or www.notlocal.com. For compatibility with other
           implementations of this, .local.com will be considered to be the same as
           local.com. A single * is the only valid wildcard, and effectively
           disables the use of proxy. */
        NOPROXY = CURLOPTTYPE.STRINGPOINT + 177,

        /* block size for TFTP transfers */
        TFTP_BLKSIZE = CURLOPTTYPE.LONG + 178,

        /* Socks Service */
        SOCKS5_GSSAPI_SERVICE = CURLOPTTYPE.STRINGPOINT + 179, /* DEPRECATED, do not use! ,

        /* Socks Service */
        SOCKS5_GSSAPI_NEC = CURLOPTTYPE.LONG + 180,

        /* set the bitmask for the protocols that are allowed to be used for the
           transfer, which thus helps the app which takes URLs from users or other
           external inputs and want to restrict what protocol(s) to deal
           with. Defaults to CURLPROTO_ALL. */
        PROTOCOLS = CURLOPTTYPE.LONG + 181,

        /* set the bitmask for the protocols that libcurl is allowed to follow to,
           as a subset of the PROTOCOLS ones. That means the protocol needs
           to be set in both bitmasks to be allowed to get redirected to. */
        REDIR_PROTOCOLS = CURLOPTTYPE.LONG + 182,

        /* set the SSH knownhost file name to use */
        SSH_KNOWNHOSTS = CURLOPTTYPE.STRINGPOINT + 183,

        /* set the SSH host key callback, must point to a curl_sshkeycallback
           function */
        SSH_KEYFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 184,

        /* set the SSH host key callback custom pointer */
        SSH_KEYDATA = CURLOPTTYPE.OBJECTPOINT + 185,

        /* set the SMTP mail originator */
        MAIL_FROM = CURLOPTTYPE.STRINGPOINT + 186,

        /* set the list of SMTP mail receiver(s) */
        MAIL_RCPT = CURLOPTTYPE.SLISTPOINT + 187,

        /* FTP: send PRET before PASV */
        FTP_USE_PRET = CURLOPTTYPE.LONG + 188,

        /* RTSP request method (OPTIONS, SETUP, PLAY, etc...) */
        RTSP_REQUEST = CURLOPTTYPE.LONG + 189,

        /* The RTSP session identifier */
        RTSP_SESSION_ID = CURLOPTTYPE.STRINGPOINT + 190,

        /* The RTSP stream URI */
        RTSP_STREAM_URI = CURLOPTTYPE.STRINGPOINT + 191,

        /* The Transport: header to use in RTSP requests */
        RTSP_TRANSPORT = CURLOPTTYPE.STRINGPOINT + 192,

        /* Manually initialize the client RTSP CSeq for this handle */
        RTSP_CLIENT_CSEQ = CURLOPTTYPE.LONG + 193,

        /* Manually initialize the server RTSP CSeq for this handle */
        RTSP_SERVER_CSEQ = CURLOPTTYPE.LONG + 194,

        /* The stream to pass to INTERLEAVEFUNCTION. */
        INTERLEAVEDATA = CURLOPTTYPE.OBJECTPOINT + 195,

        /* Let the application define a custom write method for RTP data */
        INTERLEAVEFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 196,

        /* Turn on wildcard matching */
        WILDCARDMATCH = CURLOPTTYPE.LONG + 197,

        /* Directory matching callback called before downloading of an
           individual file (chunk) started */
        CHUNK_BGN_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 198,

        /* Directory matching callback called after the file (chunk)
           was downloaded, or skipped */
        CHUNK_END_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 199,

        /* Change match (fnmatch-like) callback for wildcard matching */
        FNMATCH_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 200,

        /* Let the application define custom chunk data pointer */
        CHUNK_DATA = CURLOPTTYPE.OBJECTPOINT + 201,

        /* FNMATCH_FUNCTION user pointer */
        FNMATCH_DATA = CURLOPTTYPE.OBJECTPOINT + 202,

        /* send linked-list of name:port:address sets */
        RESOLVE = CURLOPTTYPE.SLISTPOINT + 203,

        /* Set a username for authenticated TLS */
        TLSAUTH_USERNAME = CURLOPTTYPE.STRINGPOINT + 204,

        /* Set a password for authenticated TLS */
        TLSAUTH_PASSWORD = CURLOPTTYPE.STRINGPOINT + 205,

        /* Set authentication type for authenticated TLS */
        TLSAUTH_TYPE = CURLOPTTYPE.STRINGPOINT + 206,

        /* Set to 1 to enable the "TE:" header in HTTP requests to ask for
           compressed transfer-encoded responses. Set to 0 to disable the use of TE:
           in outgoing requests. The current default is 0, but it might change in a
           future libcurl release.

           libcurl will ask for the compressed methods it knows of, and if that
           isn't any, it will not ask for transfer-encoding at all even if this
           option is set to 1.

        */
        TRANSFER_ENCODING = CURLOPTTYPE.LONG + 207,

        /* Callback function for closing socket (instead of close(2)). The callback
           should have type curl_closesocket_callback */
        CLOSESOCKETFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 208,
        CLOSESOCKETDATA = CURLOPTTYPE.OBJECTPOINT + 209,

        /* allow GSSAPI credential delegation */
        GSSAPI_DELEGATION = CURLOPTTYPE.LONG + 210,

        /* Set the name servers to use for DNS resolution */
        DNS_SERVERS = CURLOPTTYPE.STRINGPOINT + 211,

        /* Time-out accept operations (currently for FTP only) after this amount
           of milliseconds. */
        ACCEPTTIMEOUT_MS = CURLOPTTYPE.LONG + 212,

        /* Set TCP keepalive */
        TCP_KEEPALIVE = CURLOPTTYPE.LONG + 213,

        /* non-universal keepalive knobs (Linux, AIX, HP-UX, more) */
        TCP_KEEPIDLE = CURLOPTTYPE.LONG + 214,
        TCP_KEEPINTVL = CURLOPTTYPE.LONG + 215,

        /* Enable/disable specific SSL features with a bitmask, see CURLSSLOPT_* */
        SSL_OPTIONS = CURLOPTTYPE.LONG + 216,

        /* Set the SMTP auth originator */
        MAIL_AUTH = CURLOPTTYPE.STRINGPOINT + 217,

        /* Enable/disable SASL initial response */
        SASL_IR = CURLOPTTYPE.LONG + 218,

        /* Function that will be called instead of the internal progress display
         * function. This function should be defined as the curl_xferinfo_callback
         * prototype defines. (Deprecates PROGRESSFUNCTION) */
        XFERINFOFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 219,

        /* The XOAUTH2 bearer token */
        XOAUTH2_BEARER = CURLOPTTYPE.STRINGPOINT + 220,

        /* Set the interface string to use as outgoing network
         * interface for DNS requests.
         * Only supported by the c-ares DNS backend */
        DNS_INTERFACE = CURLOPTTYPE.STRINGPOINT + 221,

        /* Set the local IPv4 address to use for outgoing DNS requests.
         * Only supported by the c-ares DNS backend */
        DNS_LOCAL_IP4 = CURLOPTTYPE.STRINGPOINT + 222,

        /* Set the local IPv6 address to use for outgoing DNS requests.
         * Only supported by the c-ares DNS backend */
        DNS_LOCAL_IP6 = CURLOPTTYPE.STRINGPOINT + 223,

        /* Set authentication options directly */
        LOGIN_OPTIONS = CURLOPTTYPE.STRINGPOINT + 224,

        /* Enable/disable TLS NPN extension (http2 over ssl might fail without) */
        SSL_ENABLE_NPN = CURLOPTTYPE.LONG + 225,

        /* Enable/disable TLS ALPN extension (http2 over ssl might fail without) */
        SSL_ENABLE_ALPN = CURLOPTTYPE.LONG + 226,

        /* Time to wait for a response to a HTTP request containing an
         * Expect: 100-continue header before sending the data anyway. */
        EXPECT_100_TIMEOUT_MS = CURLOPTTYPE.LONG + 227,

        /* This points to a linked list of headers used for proxy requests only,
           struct curl_slist kind */
        PROXYHEADER = CURLOPTTYPE.SLISTPOINT + 228,

        /* Pass in a bitmask of "header options" */
        HEADEROPT = CURLOPTTYPE.LONG + 229,

        /* The public key in DER form used to validate the peer public key
           this option is used only if SSL_VERIFYPEER is true */
        PINNEDPUBLICKEY = CURLOPTTYPE.STRINGPOINT + 230,

        /* Path to Unix domain socket */
        UNIX_SOCKET_PATH = CURLOPTTYPE.STRINGPOINT + 231,

        /* Set if we should verify the certificate status. */
        SSL_VERIFYSTATUS = CURLOPTTYPE.LONG + 232,

        /* Set if we should enable TLS false start. */
        SSL_FALSESTART = CURLOPTTYPE.LONG + 233,

        /* Do not squash dot-dot sequences */
        PATH_AS_IS = CURLOPTTYPE.LONG + 234,

        /* Proxy Service Name */
        PROXY_SERVICE_NAME = CURLOPTTYPE.STRINGPOINT + 235,

        /* Service Name */
        SERVICE_NAME = CURLOPTTYPE.STRINGPOINT + 236,

        /* Wait/don't wait for pipe/mutex to clarify */
        PIPEWAIT = CURLOPTTYPE.LONG + 237,

        /* Set the protocol used when curl is given a URL without a protocol */
        DEFAULT_PROTOCOL = CURLOPTTYPE.STRINGPOINT + 238,

        /* Set stream weight, 1 - 256 (default is 16) */
        STREAM_WEIGHT = CURLOPTTYPE.LONG + 239,

        /* Set stream dependency on another CURL handle */
        STREAM_DEPENDS = CURLOPTTYPE.OBJECTPOINT + 240,

        /* Set E-xclusive stream dependency on another CURL handle */
        STREAM_DEPENDS_E = CURLOPTTYPE.OBJECTPOINT + 241,

        /* Do not send any tftp option requests to the server */
        TFTP_NO_OPTIONS = CURLOPTTYPE.LONG + 242,

        /* Linked-list of host:port:connect-to-host:connect-to-port,
           overrides the URL's host:port (only for the network layer) */
        CONNECT_TO = CURLOPTTYPE.SLISTPOINT + 243,

        /* Set TCP Fast Open */
        TCP_FASTOPEN = CURLOPTTYPE.LONG + 244,

        /* Continue to send data if the server responds early with an
         * HTTP status code >= 300 */
        KEEP_SENDING_ON_ERROR = CURLOPTTYPE.LONG + 245,

        /* The CApath or CAfile used to validate the proxy certificate
           this option is used only if PROXY_SSL_VERIFYPEER is true */
        PROXY_CAINFO = CURLOPTTYPE.STRINGPOINT + 246,

        /* The CApath directory used to validate the proxy certificate
           this option is used only if PROXY_SSL_VERIFYPEER is true */
        PROXY_CAPATH = CURLOPTTYPE.STRINGPOINT + 247,

        /* Set if we should verify the proxy in ssl handshake,
           set 1 to verify. */
        PROXY_SSL_VERIFYPEER = CURLOPTTYPE.LONG + 248,

        /* Set if we should verify the Common name from the proxy certificate in ssl
         * handshake, set 1 to check existence, 2 to ensure that it matches
         * the provided hostname. */
        PROXY_SSL_VERIFYHOST = CURLOPTTYPE.LONG + 249,

        /* What version to specifically try to use for proxy.
           See CURL_SSLVERSION defines below. */
        PROXY_SSLVERSION = CURLOPTTYPE.LONG + 250,

        /* Set a username for authenticated TLS for proxy */
        PROXY_TLSAUTH_USERNAME = CURLOPTTYPE.STRINGPOINT + 251,

        /* Set a password for authenticated TLS for proxy */
        PROXY_TLSAUTH_PASSWORD = CURLOPTTYPE.STRINGPOINT + 252,

        /* Set authentication type for authenticated TLS for proxy */
        PROXY_TLSAUTH_TYPE = CURLOPTTYPE.STRINGPOINT + 253,

        /* name of the file keeping your private SSL-certificate for proxy */
        PROXY_SSLCERT = CURLOPTTYPE.STRINGPOINT + 254,

        /* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") for
           proxy */
        PROXY_SSLCERTTYPE = CURLOPTTYPE.STRINGPOINT + 255,

        /* name of the file keeping your private SSL-key for proxy */
        PROXY_SSLKEY = CURLOPTTYPE.STRINGPOINT + 256,

        /* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") for
           proxy */
        PROXY_SSLKEYTYPE = CURLOPTTYPE.STRINGPOINT + 257,

        /* password for the SSL private key for proxy */
        PROXY_KEYPASSWD = CURLOPTTYPE.STRINGPOINT + 258,

        /* Specify which SSL ciphers to use for proxy */
        PROXY_SSL_CIPHER_LIST = CURLOPTTYPE.STRINGPOINT + 259,

        /* CRL file for proxy */
        PROXY_CRLFILE = CURLOPTTYPE.STRINGPOINT + 260,

        /* Enable/disable specific SSL features with a bitmask for proxy, see
           CURLSSLOPT_* */
        PROXY_SSL_OPTIONS = CURLOPTTYPE.LONG + 261,

        /* Name of pre proxy to use. */
        PRE_PROXY = CURLOPTTYPE.STRINGPOINT + 262,

        /* The public key in DER form used to validate the proxy public key
           this option is used only if PROXY_SSL_VERIFYPEER is true */
        PROXY_PINNEDPUBLICKEY = CURLOPTTYPE.STRINGPOINT + 263,

        /* Path to an abstract Unix domain socket */
        ABSTRACT_UNIX_SOCKET = CURLOPTTYPE.STRINGPOINT + 264,

        /* Suppress proxy CONNECT response headers from user callbacks */
        SUPPRESS_CONNECT_HEADERS = CURLOPTTYPE.LONG + 265,

        /* The request target, instead of extracted from the URL */
        REQUEST_TARGET = CURLOPTTYPE.STRINGPOINT + 266,

        /* bitmask of allowed auth methods for connections to SOCKS5 proxies */
        SOCKS5_AUTH = CURLOPTTYPE.LONG + 267,

        /* Enable/disable SSH compression */
        SSH_COMPRESSION = CURLOPTTYPE.LONG + 268,

        /* Post MIME data. */
        MIMEPOST = CURLOPTTYPE.OBJECTPOINT + 269,

        /* Time to use with the TIMECONDITION. Specified in number of
           seconds since 1 Jan 1970. */
        TIMEVALUE_LARGE = CURLOPTTYPE.OFF_T + 270,

        /* Head start in milliseconds to give happy eyeballs. */
        HAPPY_EYEBALLS_TIMEOUT_MS = CURLOPTTYPE.LONG + 271,

        /* Function that will be called before a resolver request is made */
        RESOLVER_START_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 272,

        /* User data to pass to the resolver start callback. */
        RESOLVER_START_DATA = CURLOPTTYPE.OBJECTPOINT + 273,

        /* send HAProxy PROXY protocol header? */
        HAPROXYPROTOCOL = CURLOPTTYPE.LONG + 274,

        /* shuffle addresses before use when DNS returns multiple */
        DNS_SHUFFLE_ADDRESSES = CURLOPTTYPE.LONG + 275,

        /* Specify which TLS 1.3 ciphers suites to use */
        TLS13_CIPHERS = CURLOPTTYPE.STRINGPOINT + 276,
        PROXY_TLS13_CIPHERS = CURLOPTTYPE.STRINGPOINT + 277,

        /* Disallow specifying username/login in URL. */
        DISALLOW_USERNAME_IN_URL = CURLOPTTYPE.LONG + 278,

        /* DNS-over-HTTPS URL */
        DOH_URL = CURLOPTTYPE.STRINGPOINT + 279,

        /* Preferred buffer size to use for uploads */
        UPLOAD_BUFFERSIZE = CURLOPTTYPE.LONG + 280,

        /* Time in ms between connection upkeep calls for long-lived connections. */
        UPKEEP_INTERVAL_MS = CURLOPTTYPE.LONG + 281,

        /* Specify URL using CURL URL API. */
        CURLU = CURLOPTTYPE.OBJECTPOINT + 282,

        /* add trailing data just after no more data is available */
        TRAILERFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 283,

        /* pointer to be passed to HTTP_TRAILER_FUNCTION */
        TRAILERDATA = CURLOPTTYPE.OBJECTPOINT + 284,

        /* set this to 1L to allow HTTP/0.9 responses or 0L to disallow */
        HTTP09_ALLOWED = CURLOPTTYPE.LONG + 285,

        /* alt-svc control bitmask */
        ALTSVC_CTRL = CURLOPTTYPE.LONG + 286,

        /* alt-svc cache file name to possibly read from/write to */
        ALTSVC = CURLOPTTYPE.STRINGPOINT + 287,

        /* maximum age of a connection to consider it for reuse (in seconds) */
        MAXAGE_CONN = CURLOPTTYPE.LONG + 288,

        /* SASL authorisation identity */
        SASL_AUTHZID = CURLOPTTYPE.STRINGPOINT + 289,


        /* allow RCPT TO command to fail for some recipients */
        MAIL_RCPT_ALLLOWFAILS = CURLOPTTYPE.LONG + 290,

        /* the private SSL-certificate as a "blob" */
        SSLCERT_BLOB = CURLOPTTYPE.BLOB + 291,
        SSLKEY_BLOB = CURLOPTTYPE.BLOB + 292,
        PROXY_SSLCERT_BLOB = CURLOPTTYPE.BLOB + 293,
        PROXY_SSLKEY_BLOB = CURLOPTTYPE.BLOB + 294,
        ISSUERCERT_BLOB = CURLOPTTYPE.BLOB + 295,

        /* Issuer certificate for proxy */
        PROXY_ISSUERCERT = CURLOPTTYPE.STRINGPOINT + 296,
        PROXY_ISSUERCERT_BLOB = CURLOPTTYPE.BLOB + 297,

        /* the EC curves requested by the TLS client (RFC 8422, 5.1);
         * OpenSSL support via 'set_groups'/'set_curves':
         * https://www.openssl.org/docs/manmaster/man3/SSL_CTX_set1_groups.html
         */
        SSL_EC_CURVES = CURLOPTTYPE.STRINGPOINT + 298,

        /* HSTS bitmask */
        HSTS_CTRL = CURLOPTTYPE.LONG + 299,
        /* HSTS file name */
        HSTS = CURLOPTTYPE.STRINGPOINT + 300,

        /* HSTS read callback */
        HSTSREADFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 301,
        HSTSREADDATA = CURLOPTTYPE.CBPOINT + 302,

        /* HSTS write callback */
        HSTSWRITEFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 303,
        HSTSWRITEDATA = CURLOPTTYPE.CBPOINT + 304,

        /* Parameters for V4 signature */
        AWS_SIGV4 = CURLOPTTYPE.STRINGPOINT + 305,

        /* Same as CURLOPT_SSL_VERIFYPEER but for DoH (DNS-over-HTTPS) servers. */
        DOH_SSL_VERIFYPEER = CURLOPTTYPE.LONG + 306,

        /* Same as CURLOPT_SSL_VERIFYHOST but for DoH (DNS-over-HTTPS) servers. */
        DOH_SSL_VERIFYHOST = CURLOPTTYPE.LONG + 307,

        /* Same as CURLOPT_SSL_VERIFYSTATUS but for DoH (DNS-over-HTTPS) servers. */
        DOH_SSL_VERIFYSTATUS = CURLOPTTYPE.LONG + 308,

        /* The CA certificates as "blob" used to validate the peer certificate
           this option is used only if SSL_VERIFYPEER is true */
        CAINFO_BLOB = CURLOPTTYPE.BLOB + 309,

        /* The CA certificates as "blob" used to validate the proxy certificate
           this option is used only if PROXY_SSL_VERIFYPEER is true */
        PROXY_CAINFO_BLOB = CURLOPTTYPE.BLOB + 310,

        /* used by scp/sftp to verify the host's public key */
        SSH_HOST_PUBLIC_KEY_SHA256 = CURLOPTTYPE.STRINGPOINT + 311,

        /* Function that will be called immediately before the initial request
           is made on a connection (after any protocol negotiation step).  */
        PREREQFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 312,

        /* Data passed to the CURLOPT_PREREQFUNCTION callback */
        PREREQDATA = CURLOPTTYPE.CBPOINT + 313,

        /* maximum age (since creation) of a connection to consider it for reuse
         * (in seconds) */
        MAXLIFETIME_CONN = CURLOPTTYPE.LONG + 314,

        /* Set MIME option flags. */
        MIME_OPTIONS = CURLOPTTYPE.LONG + 315,

        /* set the SSH host key callback, must point to a curl_sshkeycallback
           function */
        SSH_HOSTKEYFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 316,

        /* set the SSH host key callback custom pointer */
        SSH_HOSTKEYDATA = CURLOPTTYPE.CBPOINT + 317,

        /* specify which protocols that are allowed to be used for the transfer,
           which thus helps the app which takes URLs from users or other external
           inputs and want to restrict what protocol(s) to deal with. Defaults to
           all built-in protocols. */
        PROTOCOLS_STR = CURLOPTTYPE.STRINGPOINT + 318,

        /* specify which protocols that libcurl is allowed to follow directs to */
        REDIR_PROTOCOLS_STR = CURLOPTTYPE.STRINGPOINT + 319,

        /* websockets options */
        WS_OPTIONS = CURLOPTTYPE.LONG + 320,

        /* CA cache timeout */
        CA_CACHE_TIMEOUT = CURLOPTTYPE.LONG + 321,

        /* Can leak things, gonna exit() soon */
        QUICK_EXIT = CURLOPTTYPE.LONG + 322,



        LASTENTRY /* the last unused */
    }
}
