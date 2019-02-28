using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurlUnity
{
    public class Curl : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var code = Interface.curl_global_init(0);
            Debug.LogWarning(code);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}