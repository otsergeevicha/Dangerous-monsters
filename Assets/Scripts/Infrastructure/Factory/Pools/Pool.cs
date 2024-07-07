using ContactZones;
using Enemies.AI;
using Modules;
using Plugins.MonoCache;
using Services.Factory;
using Services.SDK;
using SO;
using Spawners;
using Turrets;
using Turrets.Children;
using UnityEngine;

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
        public PoolBosses PoolBosses { get; private set; }
        public PoolBullet PoolBullet { get; private set; }
        public PoolTurrets PoolTurrets { get; private set; }
        public PoolWorkers PoolWorkers { get; private set; }
        private PoolMissiles PoolMissiles { get; set; }
        private PoolMoney PoolMoney { get; set; }
        private PoolLootBoxes PoolLootBoxes { get; set; }

        public void Construct(IGameFactory factory, PoolData poolData,
            AssistantData assistantData, EnemyData enemyData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate, TurretPlate[] turretPlates, BulletData bulletData, TurretData turretData,
            Transform[] squareLootSpawner, ISDKService sdkService, WorkerData workerData, Transform[] gemMiners,
            StorageGem storageGem, Vector3 spawnPointBoss)
        {
            _directionOperator = new DirectionOperator();
            _enemyHealthModule = new EnemyHealthModule();

            PoolAmmoBox = new PoolAmmoBoxPlayer(factory, poolData.SizeAmmoBoxPlayer);
            PoolCargoAssistant = new PoolCargoAssistant(factory, poolData, assistantData, cartridgeGuns, storageAmmoPlate);
            
            PoolMoney = new PoolMoney(factory, poolData.MaxCountMoney);
            PoolLootBoxes = new PoolLootBoxes(factory, poolData.MaxCountLootBoxes, sdkService);

            _lootSpawner = new LootSpawner(PoolMoney, PoolLootBoxes, squareLootSpawner, poolData);

            PoolEnemies = new PoolEnemies(factory, poolData, enemyData, _directionOperator, _enemyHealthModule,
                _lootSpawner);
            PoolBosses = new PoolBosses(factory, enemyData,_directionOperator, _enemyHealthModule, _lootSpawner, spawnPointBoss);
            PoolMissiles = new PoolMissiles(factory, poolData.MaxCountMissiles, bulletData);
            PoolTurrets = new PoolTurrets(factory, turretPlates, turretData, PoolMissiles);
            PoolBullet = new PoolBullet(factory, poolData.MaxCountBullets, bulletData);
            PoolWorkers = new PoolWorkers(factory, poolData.MaxCountWorkers, workerData, gemMiners, storageGem.transform.position, storageGem);
        }

        protected override void OnDisabled() => 
            _lootSpawner.Dispose();
    }
}