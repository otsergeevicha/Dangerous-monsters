using System.Linq;
using Infrastructure.Factory.Pools;
using UnityEngine;

namespace Spawners
{
    public class LootSpawner
    {
        private readonly PoolMoney _poolMoney;

        public LootSpawner(PoolMoney poolMoney) => 
            _poolMoney = poolMoney;

        public void Spawn(int enemyId, Vector3 position) => 
            _poolMoney.Moneys.FirstOrDefault(money => 
                money.isActiveAndEnabled == false)
                ?.OnActive(enemyId, position);
    }
}