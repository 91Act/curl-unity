using CurlUnity;
using System;
using System.IO;
using UnityEngine;

public class CurlTest : MonoBehaviour
{
    void Start()
    {
        var multi = new CurlMulti();

        var asset = Resources.Load<TextAsset>("urls");
        var sr = new StringReader(asset.text);

        string line;
        while ((line = sr.ReadLine()) != null)
        {
            var easy = new CurlEasy();
            easy.uri = new Uri(line);
            easy.useHttp2 = true;
            easy.debug = true;
            easy.performCallback = OnPerformCallback;
            easy.MultiPerform(multi);
        }
    }

    void OnPerformCallback(CURLE result, CurlEasy easy)
    {
        if (result == CURLE.OK)
        {
            Debug.Log($"Perform finished: {easy.uri}");
        }
    }
}