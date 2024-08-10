using System.Linq;
using Infrastructure.Factory.Pools;
using Loots;
using SO;
using UnityEngine;

namespace Spawners
{
    public class LootSpawner
    {
        private readonly Transform[] _squareLootSpawner;
        private readonly PoolLootBoxes _poolLootBoxes;
        private readonly PoolMoney _poolMoney;
        private readonly PoolData _poolData;
        private readonly float _offSet = 5f;

        public LootSpawner(PoolMoney poolMoney, PoolLootBoxes poolLootBoxes, Transform[] squareLootSpawner,
            PoolData poolData)
        {
            _poolData = poolData;
            _squareLootSpawner = squareLootSpawner;
            _poolLootBoxes = poolLootBoxes;
            _poolMoney = poolMoney;

            foreach (LootPoint lootPoint in _poolLootBoxes.LootPoints)
            {
                lootPoint.OnActive(Random.Range(0, (int)TypeLoot.Health), _poolData.PercentSpawnFreeLoot);
                lootPoint.transform.position = GetRandomPoint();
                lootPoint.OnPickUp += ReSpawnLoot;
            }
        }

        public void Dispose()
        {
            foreach (LootPoint lootPoint in _poolLootBoxes.LootPoints)
                lootPoint.OnPickUp -= ReSpawnLoot;
        }
        
        public void SpawnMoney(int enemyId, Vector3 position) =>
            _poolMoney.Moneys.FirstOrDefault(money =>
                    money.isActiveAndEnabled == false)
                ?.OnActive(enemyId, position);

        private void ReSpawnLoot()
        {
            LootPoint lootPoint = _poolLootBoxes.LootPoints.FirstOrDefault(box =>
                box.isActiveAndEnabled == false);

            if (lootPoint != null)
            {
                lootPoint.transform.position = GetRandomPoint();
                lootPoint.OnActive(Random.Range(0, (int)TypeLoot.Health), _poolData.PercentSpawnFreeLoot);
            }
        }

        private Vector3 GetRandomPoint()
        {
            float randomX = Random.Range(Mathf.Min(_squareLootSpawner[0].position.x, _squareLootSpawner[1].position.x),
                Mathf.Max(_squareLootSpawner[2].position.x, _squareLootSpawner[3].position.x));

            float randomZ = Random.Range(Mathf.Min(_squareLootSpawner[0].position.z, _squareLootSpawner[2].position.z),
                Mathf.Max(_squareLootSpawner[1].position.z, _squareLootSpawner[3].position.z));

            Vector3 newPoint = new Vector3(randomX, 0f, randomZ);

            return _poolLootBoxes.LootPoints.Any(lootPoint =>
                lootPoint.transform.position != newPoint)
                ? newPoint
                : GetCorrectOffset(newPoint);
        }

        private Vector3 GetCorrectOffset(Vector3 newPoint) => 
            new (newPoint.x + Random.Range(-_offSet, _offSet), 0f, newPoint.z + Random.Range(-_offSet, _offSet));
    }
}