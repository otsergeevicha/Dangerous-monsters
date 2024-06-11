using System.Collections.Generic;
using CameraModule;
using Enemies;
using HpBar;
using Infrastructure.Factory.Pools;
using Player.Animation;
using Player.ShootingModule;
using Plugins.MonoCache;
using RingZone;
using Services.Bank;
using Services.Inputs;
using SO;
using Triggers;
using Turrets.Children;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HeroAnimation))]
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(HeroShooting))]
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
        [HideInInspector] [SerializeField] private HeroAnimation _heroAnimation;
        [HideInInspector] [SerializeField] private HeroShooting _heroShooting;

        private IWallet _wallet;
        private IMagazine _magazine;

        public void Construct(IInputService input, IWallet wallet, HeroData heroData, PoolAmmoBoxPlayer pool,
            PoolBullet poolBullet, int maxCountBullets, EnemyRing enemyRing, List<Enemy> poolEnemies,
            HealthBar healthBar)
        {
            healthBar.Construct(transform);
            _wallet = wallet;

            _heroAnimation.Construct(heroData);
            _heroMovement.Construct(input, heroData.Speed, _heroAnimation);
            _basketPlayer.Construct(pool, heroData.SizeBasket);
            //_heroHealth.Construct(healthBar);

            _magazine = new MagazineBullets(maxCountBullets / 2);
            _weaponHolder.Construct(poolBullet, _magazine);
            _heroShooting.Construct(_heroAnimation, _heroMovement, _weaponHolder, heroData.RadiusDetection, poolEnemies, enemyRing);
        }

        public HeroAnimation AnimationController => 
            _heroAnimation;
        
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
            _heroShooting ??= Get<HeroShooting>();
            _ammoTriggers ??= Get<AmmoTriggers>();
            _lootTriggers ??= Get<LootTriggers>();
            _heroAnimation ??= Get<HeroAnimation>();
            _weaponHolder ??= ChildrenGet<WeaponHolder>();
        }

        
        public void SetShootingState(bool heroOnBase) => 
            _heroShooting.SetOnBase(heroOnBase);

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) =>
            cartridgeGun.SetPresenceCourier(true);

        public Transform GetCameraRoot() =>
            _rootCamera.transform;

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
    }
}