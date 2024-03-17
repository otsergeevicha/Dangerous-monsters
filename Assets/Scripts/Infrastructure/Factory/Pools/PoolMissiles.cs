using System.Collections.Generic;
using Ammo;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class PoolMissiles
    {
        public List<Missile> Missiles { get; private set; } = new();
        
        public PoolMissiles(IGameFactory factory, int maxCountBullets, BulletData bulletData)
        {
            for (int i = 0; i < maxCountBullets; i++)
            {
                Missile missile = factory.CreateMissile();
                missile.Construct(bulletData);
                missile.InActive();
                Missiles.Add(missile);
            }
        }
    }
}