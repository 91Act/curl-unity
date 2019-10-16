using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CurlUnity
{
    public class CurlMultiUpdater : MonoBehaviour
    {
        public bool autoStart = true;
        public bool multiThread = true;
        public int busyInterval = 1;
        public int idleInterval = 50;

        private List<CurlMulti> multiList = new List<CurlMulti>();
        private List<CurlMulti> penddingAdd = new List<CurlMulti>();
        private List<CurlMulti> penddingRemove = new List<CurlMulti>();
        private Task multiThreadTask;
        private CancellationTokenSource taskCancelSource;
        private EventWaitHandle pauseHandle;

        private bool started = false;
        private bool paused = false;
        private int lastRunning = 0;

        private static CurlMultiUpdater instance;

        public static CurlMultiUpdater Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("CurlMultiUpdater")
                    {
                        hideFlags = HideFlags.DontSave
                    };
                    DontDestroyOnLoad(go);
                    go.AddComponent<CurlMultiUpdater>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (CurlLog.Assert(instance == null, "Only one CurlMultiUpdater instance is allowed"))
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
            if (started && !multiThread && !paused)
            {
                Perform();
            }
        }

        public void ManualStart()
        {
            if (multiThread)
            {
                pauseHandle = new EventWaitHandle(!paused, EventResetMode.ManualReset);

                taskCancelSource = new CancellationTokenSource();
                var cancelToken = taskCancelSource.Token;

                multiThreadTask = Task.Run(() =>
                {
                    while (!cancelToken.IsCancellationRequested && pauseHandle.WaitOne())
                    {
                        Perform();
                        Thread.Sleep(lastRunning > 0 ? busyInterval : idleInterval);
                    }

                }, cancelToken);
            }

            started = true;
        }

        public async Task Shutdown()
        {
            if (multiThreadTask != null)
            {
                taskCancelSource.Cancel();
                pauseHandle.Set();
                await multiThreadTask;
            }

            foreach (var multi in multiList)
            {
                multi.Abort();
            }
        }

        public void Pause(bool value)
        {
            paused = value;

            if (started && multiThread)
            {
                if (paused) pauseHandle.Reset();
                else pauseHandle.Set();
            }
        }

        public void AddMulti(CurlMulti multi)
        {
            lock(this)
            {
                penddingAdd.Add(multi);
                penddingRemove.Remove(multi);

            	if (multiThread) multi.SetupLock(true);
            }
        }

        public void RemoveMulti(CurlMulti multi)
        {
            lock (this)
            {
                penddingRemove.Add(multi);
                penddingAdd.Remove(multi);

            	if (multiThread) multi.SetupLock(false);
            }
        }

        private void Perform()
        {
            lock (this)
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
}
