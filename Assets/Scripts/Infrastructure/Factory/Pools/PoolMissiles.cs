using System.Collections.Generic;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class PoolMissiles
    {
        public List<Missile> Missiles { get; private set; } = new();
        
        public PoolMissiles(IGameFactory factory, int maxCountBullets, AmmoData ammoData)
        {
            for (int i = 0; i < maxCountBullets; i++)
            {
                Missile missile = factory.CreateMissile();
                missile.Construct(ammoData);
                missile.InActive();
                Missiles.Add(missile);
            }
        }
    }
}