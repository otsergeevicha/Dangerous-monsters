using Infrastructure.Factory.Pools;
using Plugins.MonoCache;

namespace Player
{
    public class AmmoBasket : MonoCache
    {
        public PoolAmmoBox Pool { get; private set; }

        public void Construct(PoolAmmoBox pool)
        {
            pool.FirstPointPosition(transform);
            Pool = pool;
        }
    }
}