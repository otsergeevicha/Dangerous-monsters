using Player;
using Plugins.MonoCache;
using Services.Factory;
using SO;

namespace Reflex
{
    public class Pool : MonoCache
    {
        public PoolAmmoBox PoolAmmoBox { get; private set; }
        
        public void Construct(IGameFactory factory, PoolData poolData) => 
            PoolAmmoBox = new PoolAmmoBox(factory, poolData.SizeAmmoBox);
    }
}