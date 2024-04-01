using CameraModule;
using Infrastructure.Factory.Pools;
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
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private RootCamera _rootCamera;
        [SerializeField] private BasketPlayer _basketPlayer;
        [SerializeField] private AmmoTriggers _ammoTriggers;
        [SerializeField] private LootTriggers _lootTriggers;
        private IWallet _wallet;

        public void Construct(IInputService input, IWallet wallet, HeroData heroData, PoolAmmoBoxPlayer pool)
        {
            _wallet = wallet;
            
            _heroMovement.Construct(input, heroData.Speed, heroData.IdleHash, heroData.RunHash);
            _basketPlayer.Construct(pool, heroData.SizeBasket);
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
            _heroMovement = Get<HeroMovement>();
            _ammoTriggers = Get<AmmoTriggers>();
            _lootTriggers = Get<LootTriggers>();
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