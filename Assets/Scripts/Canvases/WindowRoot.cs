using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using Services.Inputs;
using SO;
using UnityEngine;

namespace Canvases
{
    [RequireComponent(typeof(Canvas))]
    public class WindowRoot : MonoCache
    {
        public void Construct(IInputService input, StoreAssistantPlate storeAssistantPlate,
            StoreTurretPlate[] storeTurretPlates, PoolData poolData,
            Pool pool)
        {
            storeAssistantPlate.Construct(poolData.MaxCountCargoAssistant, pool.PoolCargoAssistant);

            for (int i = 0; i < storeTurretPlates.Length; i++) 
                storeTurretPlates[i].Construct(pool.PoolTurrets);
        }
    }
}