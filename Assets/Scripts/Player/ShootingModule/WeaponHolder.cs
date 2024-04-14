using System.Linq;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using UnityEngine;

namespace Player.ShootingModule
{
    public class WeaponHolder : MonoCache
    {
        [SerializeField] private Weapon[] _weapons;

        public void Construct(PoolBullet poolBullet, IMagazine magazine)
        {
            foreach (Weapon weapon in _weapons)
                weapon.Construct(poolBullet, magazine);
        }

        public Weapon GetActiveGun() => 
            _weapons.FirstOrDefault(gun => 
                gun.isActiveAndEnabled);
    }
}