using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CurlUnity
{
    public class CurlMultiRegistry : MonoBehaviour
    {
        public bool autoStart = true;
        public bool multiThread = true;

        private List<CurlMulti> multiList = new List<CurlMulti>();
        private List<CurlMulti> penddingAdd = new List<CurlMulti>();
        private List<CurlMulti> penddingRemove = new List<CurlMulti>();
        private Task multiThreadTask;
        private CancellationTokenSource taskCancelSource;
        private bool started = false;
        private int lastRunning = 0;

        private static CurlMultiRegistry instance;

        public static CurlMultiRegistry Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("CurlMultiRegistry");
                    go.hideFlags = HideFlags.DontSave;
                    DontDestroyOnLoad(go);
                    go.AddComponent<CurlMultiRegistry>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (CurlLog.Assert(instance == null, "Only one CurlMultiRegistry instance is allowed"))
            {
                instance = this;
            }
        }

        private void Start()
        {
            if (autoStart) ManualStart();
        }

        private async void OnDestroy()
        {
            if (multiThread)
            {
                await Shutdown();
            }
            started = false;
            instance = null;
        }

        private void Update()
        {
            if (started && !multiThread)
            {
                Perform();
            }
        }

        public void ManualStart()
        {
            if (multiThread)
            {
                taskCancelSource = new CancellationTokenSource();
                var cancelToken = taskCancelSource.Token;

                multiThreadTask = Task.Run(() =>
                {
                    while (!cancelToken.IsCancellationRequested)
                    {
                        Perform();
                        Thread.Sleep(lastRunning > 0 ? 1 : 50);
                    }
                }, cancelToken);
            }
            started = true;
        }

        public async Task Shutdown()
        {
            taskCancelSource.Cancel();
            await multiThreadTask;
        }

        public void Reset()
        {
            foreach (var multi in multiList)
            {
                multi.Abort();
            }
        }

        public void RegisterMulti(CurlMulti multi)
        {
            penddingAdd.Add(multi);
            penddingRemove.Remove(multi);

            if (multiThread) multi.SetupLock(true);
        }

        public void UnregisterMulti(CurlMulti multi)
        {
            penddingRemove.Add(multi);
            penddingAdd.Remove(multi);

            if (multiThread) multi.SetupLock(false);
        }

        private void Perform()
        {
            if (penddingAdd.Count > 0)
            {
                multiList.AddRange(penddingAdd);
                penddingAdd.Clear();
            }

            if (penddingRemove.Count > 0)
            {
                foreach (var multi in penddingRemove)
                {
                    multiList.Remove(multi);
                }
                penddingRemove.Clear();
            }

            lastRunning = 0;
            foreach (var multi in multiList)
            {
                lastRunning += multi.Perform();
            }
        }
    }
}