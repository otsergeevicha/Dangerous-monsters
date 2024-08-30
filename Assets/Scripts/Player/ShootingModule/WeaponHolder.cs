using System.Linq;
using CameraModule;
using Canvases;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Player.ShootingModule
{
    public class WeaponHolder : MonoCache
    {
        [SerializeField] private Weapon[] _weapons;
        [SerializeField] private AudioSource _audioSource;
        
        private Hud _hud;

        public void Construct(IMagazine magazine, CameraFollow cameraFollow,
            BulletData bulletData, EffectModule effectModule, Hud hud)
        {
            _hud = hud;
            
            foreach (Weapon weapon in _weapons)
                weapon.Construct(magazine, cameraFollow, _audioSource, bulletData, effectModule);

            _hud.WeaponButtons.OnChanged += ChangeGun;
            
            Disarmed(true);
        }

        protected override void OnDisabled() => 
            _hud.WeaponButtons.OnChanged -= ChangeGun;

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

        private void ChangeGun(int currentGun)
        {
            foreach (Weapon weapon in _weapons) 
                weapon.gameObject.SetActive(false);

            _weapons[currentGun].gameObject.SetActive(true);
        }
    }
}