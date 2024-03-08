using ContactPlatforms;
using Enemies.AI;
using Plugins.MonoCache;
using Reflex;
using Services.Factory;
using SO;
using Turret;

namespace Infrastructure.Factory.Pools
{
    public class Pool : MonoCache
    {
        private DirectionOperator _directionOperator;
        public PoolAmmoBoxPlayer PoolAmmoBox { get; private set; }
        public PoolCargoAssistant PoolCargoAssistant { get; private set; }
        public PoolEnemies PoolEnemies { get; private set; }

        public void Construct(IGameFactory factory, PoolData poolData,
            AssistantData assistantData, EnemyData enemyData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate)
        {
            _directionOperator = new DirectionOperator();
            
            PoolAmmoBox = new PoolAmmoBoxPlayer(factory, poolData.SizeAmmoBoxPlayer);
            PoolCargoAssistant = new PoolCargoAssistant(factory, poolData, assistantData, cartridgeGuns, storageAmmoPlate);
            PoolEnemies = new PoolEnemies(factory, poolData, enemyData, _directionOperator);
        }
    }
}