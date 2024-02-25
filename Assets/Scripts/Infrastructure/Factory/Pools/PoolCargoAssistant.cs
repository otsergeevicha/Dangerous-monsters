using System.Collections.Generic;
using Assistant;
using ContactPlatforms;
using Services.Factory;
using SO;
using Turret;

namespace Infrastructure.Factory.Pools
{
    public class PoolCargoAssistant
    {
        public List<CargoAssistant> Assistants { get; private set; } = new();

        public PoolCargoAssistant(IGameFactory factory, PoolData poolData, AssistantData assistantData, 
            CartridgeGun[] cartridgeGuns, StorageAmmoPlate storageAmmoPlate)
        {
            int maxCountAssistant = poolData.MaxCountCargoAssistant;
            
            for (int i = 0; i < maxCountAssistant; i++)
            {
                CargoAssistant cargoAssistant = factory.CreateCargoAssistant();
                cargoAssistant.Construct(assistantData, cartridgeGuns, storageAmmoPlate);
                cargoAssistant.InActive();
                Assistants.Add(cargoAssistant);
            }
        }
    }
}