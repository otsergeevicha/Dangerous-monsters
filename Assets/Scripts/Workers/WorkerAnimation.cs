using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Workers
{
    [RequireComponent(typeof(Animator))]
    public class WorkerAnimation : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        
        private WorkerData _workerData;

        public void Construct(WorkerData workerData) => 
            _workerData = workerData;

        private void OnValidate() => 
            _animator ??= Get<Animator>();

        public void EnableSitingIdle()
        {
           // throw new System.NotImplementedException();
        }

        public void EnableRun()
        {
          //  throw new System.NotImplementedException();
        }

        public void EnableWalk()
        {
           // throw new System.NotImplementedException();
        }

        public void EnableIdle()
        {
           // throw new System.NotImplementedException();
        }
    }
}