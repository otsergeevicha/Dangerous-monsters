using System.Linq;
using CameraModule;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using UnityEngine;

namespace Player.ShootingModule
{
    public class WeaponHolder : MonoCache
    {
        [SerializeField] private Weapon[] _weapons;

        public void Construct(PoolBullet poolBullet, IMagazine magazine, CameraFollow cameraFollow)
        {
            foreach (Weapon weapon in _weapons)
                weapon.Construct(poolBullet, magazine, cameraFollow);
            
            Disarmed(true);
        }

        public Weapon GetActiveGun() => 
            _weapons.FirstOrDefault(gun => 
                gun.isActiveAndEnabled);

        public void Disarmed(bool heroOnBase)
        {
            if (heroOnBase == false) 
                _weapons[0].gameObject.SetActive(true);

            if(heroOnBase)    
            {
                foreach (Weapon weapon in _weapons) 
                    weapon.gameObject.SetActive(false);
            }
        }
    }
}