using ContactPlatforms;
using Plugins.MonoCache;
using Services.Factory;
using SO;
using Turret;

namespace Infrastructure.Factory.Pools
{
    public class Pool : MonoCache
    {
        public PoolAmmoBox PoolAmmoBox { get; private set; }
        public PoolCargoAssistant PoolCargoAssistant { get; private set; }
        
        public void Construct(IGameFactory factory, PoolData poolData, 
            AssistantData assistantData, CartridgeGun[] cartridgeGuns, StorageAmmoPlate storageAmmoPlate)
        {
            PoolAmmoBox = new PoolAmmoBox(factory, poolData.SizeAmmoBoxPlayer);
            PoolCargoAssistant = new PoolCargoAssistant(factory, poolData, assistantData, cartridgeGuns, storageAmmoPlate);
        }
    }
}