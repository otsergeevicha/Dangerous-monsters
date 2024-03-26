using ContactPlatforms;
using Enemies.AI;
using Loots;
using Plugins.MonoCache;
using Services.Factory;
using SO;
using Turrets;
using Turrets.Children;

namespace Infrastructure.Factory.Pools
{
    public class Pool : MonoCache
    {
        private DirectionOperator _directionOperator;
        private HealthOperator _healthOperator;
        private LootSpawner _lootSpawner;

        public PoolAmmoBoxPlayer PoolAmmoBox { get; private set; }
        public PoolCargoAssistant PoolCargoAssistant { get; private set; }
        public PoolEnemies PoolEnemies { get; private set; }
        public PoolMissiles PoolMissiles { get; private set; }
        public PoolTurrets PoolTurrets { get; private set; }
        public PoolMoney PoolMoney { get; private set; }

        public void Construct(IGameFactory factory, PoolData poolData,
            AssistantData assistantData, EnemyData enemyData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate, TurretPlate[] turretPlates, BulletData bulletData, TurretData turretData)
        {
            _directionOperator = new DirectionOperator();
            _healthOperator = new HealthOperator();
            
            PoolAmmoBox = new PoolAmmoBoxPlayer(factory, poolData.SizeAmmoBoxPlayer);
            PoolCargoAssistant = new PoolCargoAssistant(factory, poolData, assistantData, cartridgeGuns, storageAmmoPlate);
            PoolMoney = new PoolMoney(factory, poolData.MaxCountMoney);
            
            _lootSpawner = new LootSpawner(PoolMoney);
            
            PoolEnemies = new PoolEnemies(factory, poolData, enemyData, _directionOperator, _healthOperator, _lootSpawner);
            PoolMissiles = new PoolMissiles(factory, poolData.MaxCountBullets, bulletData);
            PoolTurrets = new PoolTurrets(factory, turretPlates, turretData, PoolMissiles);
        }
    }
}