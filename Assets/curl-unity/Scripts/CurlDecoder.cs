namespace CurlUnity
{
    public interface CurlDecoder
    {
        string DecodeOutData(CurlEasy easy);
        string DecodeInData(CurlEasy easy);
    }
}