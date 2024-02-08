using CameraModule;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using Services.Inputs;
using SO;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(HeroTriggers))]
    public class Hero : MonoCache
    {
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private RootCamera _rootCamera;
        [SerializeField] private AmmoBasket _ammoBasket;
        [SerializeField] private HeroTriggers _heroTriggers;

        public void Construct(IInputService input, HeroData heroData, PoolAmmoBox pool)
        {
            _heroMovement.Construct(input, heroData.HeroSpeed, heroData.HeroIdleHash, heroData.HeroRunHash);
            _ammoBasket.Construct(pool, heroData.SizeBasket);
        }

        protected override void OnEnabled()
        {
            _heroTriggers.StorageEntered += OnStorageEntered;
            _heroTriggers.StorageExited += OnStorageExited;

            _heroTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
            _heroTriggers.CartridgeGunExited += OnCartridgeGunExited;
        }

        protected override void OnDisabled()
        {
            _heroTriggers.StorageEntered -= OnStorageEntered;
            _heroTriggers.StorageExited -= OnStorageExited;

            _heroTriggers.CartridgeGunEntered -= OnCartridgeGunEntered;
            _heroTriggers.CartridgeGunExited -= OnCartridgeGunExited;
        }

        private void OnValidate()
        {
            _heroMovement = Get<HeroMovement>();
            _heroTriggers = Get<HeroTriggers>();
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