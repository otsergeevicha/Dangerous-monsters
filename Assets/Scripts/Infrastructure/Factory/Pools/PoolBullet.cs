using System.Collections.Generic;
using Ammo;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class PoolBullet
    {
        private readonly List<Bullet> _bullets = new();

        public IReadOnlyList<Bullet> Bullets =>
            _bullets.AsReadOnly();

        public PoolBullet(IGameFactory factory, int maxCountBullets, BulletData bulletData)
        {
            for (int i = 0; i < maxCountBullets; i++)
            {
                Bullet bullet = factory.CreateBullet();
                bullet.Construct(bulletData);
                bullet.InActive();
                _bullets.Add(bullet);
            }
        }

        public void AdaptingLevel()
        {
            foreach (Bullet bullet in _bullets) 
                bullet.InActive();
        }
    }
}