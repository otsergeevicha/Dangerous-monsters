using System;
using ContactPlatforms;
using Enemies.AI;
using Loots;
using Modules;
using Plugins.MonoCache;
using Services.Factory;
using SO;
using Spawners;
using Turrets;
using Turrets.Children;

namespace Infrastructure.Factory.Pools
{
    public class Pool : MonoCache
    {
        private DirectionOperator _directionOperator;
        private EnemyHealthModule _enemyHealthModule;
        private LootSpawner _lootSpawner;

        public PoolAmmoBoxPlayer PoolAmmoBox { get; private set; }
        public PoolCargoAssistant PoolCargoAssistant { get; private set; }
        public PoolEnemies PoolEnemies { get; private set; }
        public PoolMissiles PoolMissiles { get; private set; }
        public PoolTurrets PoolTurrets { get; private set; }
        public PoolMoney PoolMoney { get; private set; }
        public PoolBullet PoolBullet { get; private set; }

        public void Construct(IGameFactory factory, PoolData poolData,
            AssistantData assistantData, EnemyData enemyData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate, TurretPlate[] turretPlates, BulletData bulletData, TurretData turretData)
        {
            _directionOperator = new DirectionOperator();
            _enemyHealthModule = new EnemyHealthModule();

            PoolAmmoBox = new PoolAmmoBoxPlayer(factory, poolData.SizeAmmoBoxPlayer);
            PoolCargoAssistant =
                new PoolCargoAssistant(factory, poolData, assistantData, cartridgeGuns, storageAmmoPlate);
            PoolMoney = new PoolMoney(factory, poolData.MaxCountMoney);

            _lootSpawner = new LootSpawner(PoolMoney);

            PoolEnemies = new PoolEnemies(factory, poolData, enemyData, _directionOperator, _enemyHealthModule,
                _lootSpawner);
            PoolMissiles = new PoolMissiles(factory, poolData.MaxCountMissiles, bulletData);
            PoolTurrets = new PoolTurrets(factory, turretPlates, turretData, PoolMissiles);
            PoolBullet = new PoolBullet(factory, poolData.MaxCountBullets, bulletData);
        }
    }
}