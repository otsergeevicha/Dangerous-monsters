using CameraModule;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using Services.Inputs;
using SO;
using Turret;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(AmmoTriggers))]
    public class Hero : MonoCache
    {
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private RootCamera _rootCamera;
        [SerializeField] private AmmoBasket _ammoBasket;
        [SerializeField] private AmmoTriggers _ammoTriggers;

        public void Construct(IInputService input, HeroData heroData, PoolAmmoBox pool)
        {
            _heroMovement.Construct(input, heroData.Speed, heroData.IdleHash, heroData.RunHash);
            _ammoBasket.Construct(pool, heroData.SizeBasket);
        }

        protected override void OnEnabled()
        {
            _ammoTriggers.StorageEntered += OnStorageEntered;
            _ammoTriggers.StorageExited += OnStorageExited;

            _ammoTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited += OnCartridgeGunExited;
        }

        protected override void OnDisabled()
        {
            _ammoTriggers.StorageEntered -= OnStorageEntered;
            _ammoTriggers.StorageExited -= OnStorageExited;

            _ammoTriggers.CartridgeGunEntered -= OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited -= OnCartridgeGunExited;
        }

        private void OnValidate()
        {
            _heroMovement = Get<HeroMovement>();
            _ammoTriggers = Get<AmmoTriggers>();
        }

        private void OnStorageEntered() =>
            _ammoBasket.Replenishment().Forget();

        private void OnStorageExited() =>
            _ammoBasket.StopReplenishment();

        private void OnCartridgeGunEntered(CartridgeGun cartridgeGun)
        {
            if (_ammoBasket.IsEmpty)
                return;

            cartridgeGun.SetPresenceCourier(false);
            cartridgeGun.ApplyBox(_ammoBasket);
        }

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) => 
            cartridgeGun.SetPresenceCourier(true);

        public Transform GetCameraRoot() =>
            _rootCamera.transform;
    }
}