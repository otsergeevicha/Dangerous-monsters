using Plugins.MonoCache;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class Pool : MonoCache
    {
        public PoolAmmoBox PoolAmmoBox { get; private set; }
        
        public void Construct(IGameFactory factory, PoolData poolData) => 
            PoolAmmoBox = new PoolAmmoBox(factory, poolData.SizeAmmoBox);
    }
}