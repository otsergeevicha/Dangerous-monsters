using System.Linq;
using CameraModule;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Player.ShootingModule
{
    public class WeaponHolder : MonoCache
    {
        [SerializeField] private Weapon[] _weapons;
        [SerializeField] private AudioSource _audioSource;

        public void Construct(IMagazine magazine, CameraFollow cameraFollow,
            BulletData bulletData)
        {
            foreach (Weapon weapon in _weapons)
                weapon.Construct(magazine, cameraFollow, _audioSource, bulletData);
            
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