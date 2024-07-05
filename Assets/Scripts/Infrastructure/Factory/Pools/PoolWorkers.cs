using System.Collections.Generic;
using Services.Factory;
using SO;
using UnityEngine;
using Workers;

namespace Infrastructure.Factory.Pools
{
    public class PoolWorkers
    {
        private readonly List<Worker> _workers = new();

        public IReadOnlyList<Worker> Workers =>
            _workers.AsReadOnly();

        public PoolWorkers(IGameFactory factory, int maxCountWorkers, WorkerData workerData, Transform[] gemMiners,
            Vector3 storageGem)
        {
            for (int i = 0; i < maxCountWorkers; i++)
            {
                Worker worker = factory.CreateWorker();
                worker.Construct(workerData, gemMiners, storageGem);
                worker.InActive();
                _workers.Add(worker);
            }
        }
    }
}