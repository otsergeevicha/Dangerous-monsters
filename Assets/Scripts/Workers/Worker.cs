using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Workers
{
    [RequireComponent(typeof(WorkerAnimation))]
    public class Worker : MonoCache
    {
        [SerializeField] private Transform _helmet;
        [SerializeField] private Transform _trousers;
        [SerializeField] private Transform _vest;
        [SerializeField] private Transform _hummer;
        [SerializeField] private Transform _gem;

        public void Construct(WorkerData workerData)
        {
            WorkerData = workerData;

            WorkerAnimation = Get<WorkerAnimation>();
            WorkerAnimation.Construct(workerData);
        }
        
        public Vector3 Workplace { get; private set; }
        public WorkerData WorkerData { get; private set; }
        public WorkerAnimation WorkerAnimation { get; private set; }
        public bool IsReadyWork { get; private set; }
        public bool AtWork { get; private set; }
        public bool IsStorageEmpty { get; set; }
        public bool IsHandEmpty { get; set; }

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

        public void OnAtWork()
        {
            AtWork = true;

            _helmet.gameObject.SetActive(true);
            _trousers.gameObject.SetActive(true);
            _vest.gameObject.SetActive(true);
            _hummer.gameObject.SetActive(true);
        }

        private void OnHit()
        {
            
        }
    }
}