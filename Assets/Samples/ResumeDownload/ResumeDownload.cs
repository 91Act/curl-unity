using CurlUnity;
using System;
using System.IO;
using UnityEngine;

public class ResumeDownload : MonoBehaviour
{
    public string m_link;
    public string m_localPath;

    private long m_downloadedBytes;
    private long m_nowBytes;
    private long m_totalBytes;

    private CurlMulti m_multi;

    void Start()
    {
        m_multi = new CurlMulti();
        StartDownload();
    }

    async void StartDownload()
    {
        var easy = new CurlEasy();
        easy.uri = new Uri(m_link);
        easy.method = "HEAD";
        easy.debug = true;

        if (await easy.PerformAsync() == CURLE.OK)
        {
            if (easy.GetResponseHeader("Accept-Ranges") == "bytes")
            {
                m_downloadedBytes = 0;
                if (File.Exists(m_localPath)) m_downloadedBytes = (new FileInfo(m_localPath)).Length;
                int.TryParse(easy.GetResponseHeader("Content-Length"), out var contentLength);
                if (contentLength != 0 && contentLength <= m_downloadedBytes)
                {
                    Debug.LogWarning($"Link {m_link} already downloaded at: {m_localPath}");
                }
                else
                {
                    Debug.Log($"Start to download {m_link} in range {m_downloadedBytes}-{contentLength}");
                    easy.method = "GET";
                    easy.outputPath = m_localPath;
                    easy.rangeStart = m_downloadedBytes;
                    easy.rangeEnd = contentLength - 1;
                    easy.progressCallback = (dltotal, dlnow, ultotal, ulnow, _) =>
                    {
                        m_nowBytes = dlnow;
                        m_totalBytes = dltotal;
                    };
                    easy.performCallback = (result, _) =>
                    {
                        if (result == CURLE.OK)
                        {
                            Debug.Log($"Link {m_link} downloaded");
                        }
                        else
                        {
                            Debug.LogWarning($"Failed to download link {m_link}");
                        }
                    };
                    // Use multi perform so we could interrupt the download
                    easy.MultiPerform(m_multi);
                }
            }
            else
            {
                Debug.LogWarning($"Link {m_link} doesn't support Accept-Ranges");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to get info of link {m_link}");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label($"Download progress {m_nowBytes}-{m_totalBytes} ({m_downloadedBytes} already downloaded)");
    }
}
