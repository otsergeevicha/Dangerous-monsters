using System.Collections.Generic;
using CameraModule;
using Canvases;
using ContactZones;
using Enemies;
using HpBar;
using Infrastructure.Factory.Pools;
using Modules;
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
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private RootCamera _rootCamera;
        [SerializeField] private BasketPlayer _basketPlayer;
        [SerializeField] private AmmoTriggers _ammoTriggers;
        [SerializeField] private LootTriggers _lootTriggers;
        [SerializeField] private WeaponHolder _weaponHolder;
        [SerializeField] private HeroAnimation _heroAnimation;
        [SerializeField] private HeroShooting _heroShooting;

        [SerializeField] private GameObject _healingEffect;

        private IWallet _wallet;
        private IMagazine _magazine;
        
        private HeroHealthModule _heroHealthModule;
        private WindowModule _windowModule;
        private List<Enemy> _poolEnemies;
        private List<Enemy> _poolBosses;
        private HeroData _heroData;
        private HeroAimRing _aimRing;

        public void Construct(IInputService input, IWallet wallet, HeroData heroData, PoolAmmoBoxPlayer pool,
            PoolBullet poolBullet, int maxCountBullets, EnemyRing enemyRing, List<Enemy> poolEnemies,
            List<Enemy> poolBosses,
            HealthBar healthBar, Hud hud, WindowModule windowModule, CameraFollow cameraFollow, HeroAimRing aimRing)
        {
            _aimRing = aimRing;
            _heroData = heroData;
            _poolBosses = poolBosses;
            _poolEnemies = poolEnemies;
            _windowModule = windowModule;
            _wallet = wallet;
            
            _aimRing.MagnetEffect.Construct(this);
            healthBar.Construct(transform);
            _heroHealthModule = new HeroHealthModule(healthBar, heroData);
            _heroHealthModule.Died += OnDied;

            _heroAnimation.Construct(heroData);
            _heroMovement.Construct(input, heroData.Speed, _heroAnimation);
            _basketPlayer.Construct(pool, heroData.SizeBasket);

            _magazine = new MagazineBullets(maxCountBullets / 2, hud);
            _weaponHolder.Construct(poolBullet, _magazine, cameraFollow);
            _heroShooting.Construct(_heroMovement, _weaponHolder, heroData.RadiusDetection, enemyRing);
            _heroShooting.MergeEnemies(poolEnemies, poolBosses);
            
            _healingEffect.SetActive(false);
        }

        public HeroAnimation AnimationController => 
            _heroAnimation;

        protected override void OnEnabled()
        {
            _ammoTriggers.StorageEntered += OnStorageEntered;
            _ammoTriggers.StorageExited += OnStorageExited;

            _ammoTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited += OnCartridgeGunExited;
            
            _lootTriggers.StorageGemEntered += OnStorageGemEntered;
            _lootTriggers.StorageGemExited += OnStorageGemExited;

            _lootTriggers.OnPickUpMoney += ApplyMoney;

        }

        protected override void OnDisabled()
        {
            _ammoTriggers.StorageEntered -= OnStorageEntered;
            _ammoTriggers.StorageExited -= OnStorageExited;

            _ammoTriggers.CartridgeGunEntered -= OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited -= OnCartridgeGunExited;
            
            _lootTriggers.StorageGemEntered -= OnStorageGemEntered;
            _lootTriggers.StorageGemExited -= OnStorageGemExited;

            _lootTriggers.OnPickUpMoney -= ApplyMoney;
            
            _heroHealthModule.Died -= OnDied;
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

        public Transform GetCameraRoot() =>
            _rootCamera.transform;

        public void ApplyDamage(int damage) => 
            _heroHealthModule.ApplyDamage(damage);

        public void OnHealing()
        {
            _healingEffect.SetActive(false);
            _healingEffect.SetActive(true);
            _heroHealthModule.Reset();
        }

        public void Upgrade()
        {
            _heroHealthModule.Reset();
            _heroMovement.Upgrade(_heroData.Speed);
            _basketPlayer.Upgrade(_heroData.SizeBasket);
            _heroShooting.Upgrade(_heroData.RadiusDetection);
            _aimRing.ChangeRadius(_heroData.RadiusDetection);
        }

        public void ApplyMoney(int money) =>
            _wallet.ApplyMoney(money);

        public void ApplyGem(int gem) =>
            _wallet.ApplyGem(gem);

        public void UpdateLevel()
        {
            _heroShooting.SetOnBase(true);
            _heroMovement.SetStartPosition();
            _heroMovement.SetStateBattle(false, null);
            _heroShooting.MergeEnemies(_poolEnemies, _poolBosses);
            _heroAnimation.EnableIdle();
            _magazine.UpdateLevel();
        }

        public void OnMagnetEffect() =>
            _aimRing.MagnetEffect.OnActive();

        private void OnDied() => 
            _windowModule.HeroDied();

        private void OnStorageEntered() =>
            _basketPlayer.Replenishment().Forget();

        private void OnStorageExited() =>
            _basketPlayer.StopReplenishment();

        private void OnStorageGemEntered(StorageGem storageGem)
        {
            if (storageGem.IsEmpty)
                return;

            storageGem.GetGem(_wallet.ApplyGem).Forget();
        }

        private void OnStorageGemExited(StorageGem storageGem) => 
            storageGem.HeroOut();

        private void OnCartridgeGunEntered(CartridgeGun cartridgeGun)
        {
            if (_basketPlayer.IsEmpty)
                return;

            cartridgeGun.SetPresenceCourier(false);
            cartridgeGun.ApplyBox(_basketPlayer);
        }

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) =>
            cartridgeGun.SetPresenceCourier(true);
    }
}