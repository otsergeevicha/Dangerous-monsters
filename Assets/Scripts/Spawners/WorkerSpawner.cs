using Canvases;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using UnityEngine;

namespace Spawners
{
    public class WorkerSpawner : MonoCache
    {
        [SerializeField] private WorkerSpawnPoint[] _workerSpawnPoints = new WorkerSpawnPoint[4];

        public void Construct(PoolWorkers pool, Vector3 workplace)
        {
            foreach (WorkerSpawnPoint spawnPoint in _workerSpawnPoints) 
                spawnPoint.Construct(pool, workplace);
        }

        public void UpdateLevel()
        {
            foreach (WorkerSpawnPoint spawnPoint in _workerSpawnPoints)
                spawnPoint.OnActiveSpawner();
        }
    }
}