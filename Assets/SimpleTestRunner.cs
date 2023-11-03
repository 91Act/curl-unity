using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CurlUnity;

public class SimpleTestRunner : MonoBehaviour
{
    public Text text;
    string log = "starting test...";

    // Start is called before the first frame update
    void Start()
    {
        text.text = log;

        var version = CurlEasy.Version();
        Debug.Log("curl version: " + version);
        log += $"\nversion=${version}";
        text.text = log;

        FetchPage("http://www.google.com/");
        FetchPage("https://www.google.com/");
    }

    void FetchPage(string url)
    {
        CurlEasy easy = new CurlEasy();
        easy.uri = new System.Uri(url);

        var e = easy.Perform();
        log += $"\nurl={url}, error={e}, version={easy.httpVersion}, status={easy.status}, response={easy.inText.Substring(0, 20)}";

        text.text = log;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
