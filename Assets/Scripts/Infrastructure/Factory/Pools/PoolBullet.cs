using System.Collections.Generic;
using Ammo;
using Player.ShootingModule;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class PoolBullet
    {
        public List<Bullet> Bullets { get; private set; } = new();

        public PoolBullet(IGameFactory factory, int maxCountBullets, BulletData bulletData)
        {
            for (int i = 0; i < maxCountBullets; i++)
            {
                Bullet bullet = factory.CreateBullet();
                bullet.Construct(bulletData);
                bullet.InActive();
                Bullets.Add(bullet);
            }
        }
    }
}