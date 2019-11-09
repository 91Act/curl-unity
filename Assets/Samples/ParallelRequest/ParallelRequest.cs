using CurlUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelRequest : MonoBehaviour
{
    public string[] m_links;

    private CurlMulti multi;

    void Start()
    {
        multi = new CurlMulti();
        foreach(var link in m_links)
        {
            var easy = new CurlEasy();
            easy.uri = new Uri(link);
            easy.debug = true;
            easy.connectionTimeout = 3000;
            easy.MultiPerform(multi);
        }
    }
}
