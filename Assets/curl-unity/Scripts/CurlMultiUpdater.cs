using System.Collections.Generic;
using System.Linq;

using UnityEngine;
namespace CurlUnity
{
    public class CurlMultiUpdater : MonoBehaviour
    {
        private List<CurlMulti> multiList = new List<CurlMulti>();
        private List<CurlMulti> penddingAdd = new List<CurlMulti>();
        private List<CurlMulti> penddingRemove = new List<CurlMulti>();

        private static CurlMultiUpdater instance;

        public static CurlMultiUpdater Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("CurlMultiUpdater");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    instance = go.AddComponent<CurlMultiUpdater>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        public void AddMulti(CurlMulti multi)
        {
            penddingAdd.Add(multi);
            penddingRemove.Remove(multi);
        }

        public void RemoveMulti(CurlMulti multi)
        {
            penddingRemove.Add(multi);
            penddingAdd.Remove(multi);
        }

        void Update()
        {
            if (penddingAdd.Count > 0)
            {
                multiList.AddRange(penddingAdd);
                penddingAdd.Clear();
            }

            if (penddingRemove.Count > 0)
            {
                foreach(var entry in penddingRemove)
                {
                    multiList.Remove(entry);
                }
                penddingRemove.Clear();
            }

            foreach (var entry in multiList)
            {
                entry.Tick();
            }
        }
    }
}