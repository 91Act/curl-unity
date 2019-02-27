#include <stdio.h>
#include <memory.h>
#include <stdlib.h>
#include "curl/curl.h"

struct MemoryStream
{
    char* pBuffer;
    size_t size;
    ~MemoryStream()
    {
        if (pBuffer != nullptr)
        {
            free(pBuffer);
            pBuffer = nullptr;
        }
    }
    static size_t WriteCallback(char *ptr, size_t size, size_t nmemb, void *userdata) 
    {
        MemoryStream* pStream = (MemoryStream*)userdata;
        size *= nmemb;
        if (pStream != nullptr && size > 0)
        {
            size_t newSize = pStream->size + size + 1;
            pStream->pBuffer = (char*)realloc((void*)pStream->pBuffer, newSize);
            memcpy(pStream->pBuffer + pStream->size, ptr, size);
            pStream->size += size;
            pStream->pBuffer[pStream->size] = 0;
        }
        return size;
    }
};

int main(int argc, char* argv[])
{
    CURL* handle = NULL;
    handle = curl_easy_init();
    MemoryStream stream = {0};
    curl_easy_setopt(handle, CURLOPT_HTTP_VERSION, CURL_HTTP_VERSION_2_0);
    curl_easy_setopt(handle, CURLOPT_VERBOSE, 1);
    curl_easy_setopt(handle, CURLOPT_URL, "https://nghttp2.org/");
    curl_easy_setopt(handle, CURLOPT_HEADER, 1);
    curl_easy_setopt(handle, CURLOPT_SSL_VERIFYPEER, 0);
    curl_easy_setopt(handle, CURLOPT_SSL_VERIFYHOST, 0);
    curl_easy_setopt(handle, CURLOPT_WRITEFUNCTION, &MemoryStream::WriteCallback); 
    curl_easy_setopt(handle, CURLOPT_WRITEDATA, (void*)&stream);
    curl_easy_perform(handle);

    printf("%s", stream.pBuffer);

    return 0;
}