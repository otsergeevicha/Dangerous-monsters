using System;
using CameraModule;
using Infrastructure.Factory.Pools;
using Player.ShootingModule;
using Plugins.MonoCache;
using Services.Bank;
using Services.Inputs;
using SO;
using Triggers;
using Turrets.Children;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(AmmoTriggers))]
    [RequireComponent(typeof(LootTriggers))]
    public class Hero : MonoCache
    {
        [HideInInspector] [SerializeField] private HeroMovement _heroMovement;
        [HideInInspector] [SerializeField] private RootCamera _rootCamera;
        [HideInInspector] [SerializeField] private BasketPlayer _basketPlayer;
        [HideInInspector] [SerializeField] private AmmoTriggers _ammoTriggers;
        [HideInInspector] [SerializeField] private LootTriggers _lootTriggers;
        [HideInInspector] [SerializeField] private WeaponHolder _weaponHolder;

        private IWallet _wallet;
        private IMagazine _magazine;
        
        private HeroData _heroData;

        public void Construct(IInputService input, IWallet wallet, HeroData heroData, PoolAmmoBoxPlayer pool,
            PoolBullet poolBullet, int maxCountBullets)
        {
            _heroData = heroData;
            _wallet = wallet;

            _heroMovement.Construct(input, heroData.Speed, heroData.IdleHash, heroData.RunHash);
            _basketPlayer.Construct(pool, heroData.SizeBasket);

            _magazine = new MagazineBullets(maxCountBullets / 2);
            _weaponHolder.Construct(poolBullet, _magazine);
        }

        protected override void OnEnabled()
        {
            _ammoTriggers.StorageEntered += OnStorageEntered;
            _ammoTriggers.StorageExited += OnStorageExited;

            _ammoTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited += OnCartridgeGunExited;

            _lootTriggers.OnPickUpMoney += ApplyMoney;
        }

        protected override void OnDisabled()
        {
            _ammoTriggers.StorageEntered -= OnStorageEntered;
            _ammoTriggers.StorageExited -= OnStorageExited;

            _ammoTriggers.CartridgeGunEntered -= OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited -= OnCartridgeGunExited;

            _lootTriggers.OnPickUpMoney -= ApplyMoney;
        }

        private void OnValidate()
        {
            _heroMovement ??= Get<HeroMovement>();
            _ammoTriggers ??= Get<AmmoTriggers>();
            _lootTriggers ??= Get<LootTriggers>();
            _weaponHolder ??= ChildrenGet<WeaponHolder>();
        }

        public void SetActualRunHash(bool heroOnBase)
        {
            _heroMovement.SetRunHash(heroOnBase
                ? _heroData.RunHash
                : _heroData.RunGunHash);
        }
        
        private void ApplyMoney(int money) =>
            _wallet.ApplyMoney(money);

        private void OnStorageEntered() =>
            _basketPlayer.Replenishment().Forget();

        private void OnStorageExited() =>
            _basketPlayer.StopReplenishment();

        private void OnCartridgeGunEntered(CartridgeGun cartridgeGun)
        {
            if (_basketPlayer.IsEmpty)
                return;

            cartridgeGun.SetPresenceCourier(false);
            cartridgeGun.ApplyBox(_basketPlayer);
        }

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) =>
            cartridgeGun.SetPresenceCourier(true);

        public Transform GetCameraRoot() =>
            _rootCamera.transform;
    }
}