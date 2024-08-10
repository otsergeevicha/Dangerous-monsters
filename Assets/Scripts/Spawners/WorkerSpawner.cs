using System;
using Canvases;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using UnityEngine;

namespace Spawners
{
    public class WorkerSpawner : MonoCache
    {
        [SerializeField] private Transform _markerPosition;
        [SerializeField] private Transform _rootCamera;
        [SerializeField] private WorkerSpawnPoint[] _workerSpawnPoints = new WorkerSpawnPoint[4];

        public event Action OnTutorialContacted;
        
        public void Construct(PoolWorkers pool, Vector3 workplace)
        {
            foreach (WorkerSpawnPoint spawnPoint in _workerSpawnPoints) 
                spawnPoint.Construct(pool, workplace, this);
        }

        public void UpdateLevel()
        {
            foreach (WorkerSpawnPoint spawnPoint in _workerSpawnPoints)
                spawnPoint.OnActiveSpawner();
        }

        public void Notify() => 
            OnTutorialContacted?.Invoke();

        public Vector3 GetPositionMarker() => 
            _markerPosition.transform.position;

        public Transform GetRootCamera() => 
            _rootCamera;
    }
}