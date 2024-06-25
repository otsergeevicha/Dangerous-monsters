using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Workers
{
    public class Worker : MonoCache
    {
        public Vector3 Workplace { get; private set; }
        public WorkerData WorkerData { get; private set; }
        public WorkerAnimation WorkerAnimation { get; private set; }
        public bool IsReadyWork { get; private set; }
        public bool AtWork { get; private set; }
        public bool IsStorageEmpty { get; set; }
        public bool IsHandEmpty { get; set; }

        public void Construct(WorkerData workerData)
        {
            WorkerData = workerData;

            WorkerAnimation = Get<WorkerAnimation>();
            WorkerAnimation.Construct(workerData);
        }

        public void OnActive() => 
            gameObject.SetActive(true);

        public void InActive() => 
            gameObject.SetActive(false);

        public void SendWorkplace(Vector3 workplace)
        {
            Workplace = workplace;
            IsReadyWork = true;
        }

        public void Reset()
        {
            IsReadyWork = false;
            AtWork = false;
        }

        public Vector3 GetFreeGemPosition()
        {
            throw new System.NotImplementedException();
        }
    }
}