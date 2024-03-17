using ContactPlatforms;
using Enemies.AI;
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
        
        public PoolAmmoBoxPlayer PoolAmmoBox { get; private set; }
        public PoolCargoAssistant PoolCargoAssistant { get; private set; }
        public PoolEnemies PoolEnemies { get; private set; }
        public PoolMissiles PoolMissiles { get; private set; }
        public PoolTurrets PoolTurrets { get; private set; }

        public void Construct(IGameFactory factory, PoolData poolData,
            AssistantData assistantData, EnemyData enemyData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate, TurretPlate[] turretPlates, BulletData bulletData, TurretData turretData)
        {
            _directionOperator = new DirectionOperator();
            
            PoolAmmoBox = new PoolAmmoBoxPlayer(factory, poolData.SizeAmmoBoxPlayer);
            PoolCargoAssistant = new PoolCargoAssistant(factory, poolData, assistantData, cartridgeGuns, storageAmmoPlate);
            PoolEnemies = new PoolEnemies(factory, poolData, enemyData, _directionOperator);
            PoolMissiles = new PoolMissiles(factory, poolData.MaxCountBullets, bulletData);
            PoolTurrets = new PoolTurrets(factory, turretPlates, turretData, PoolMissiles);
        }
    }
}