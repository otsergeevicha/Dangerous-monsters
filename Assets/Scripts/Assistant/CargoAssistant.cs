using System.Linq;
using ContactPlatforms;
using Infrastructure.Factory.Pools;
using Player;
using Plugins.MonoCache;
using SO;
using Turret;
using UnityEngine;

namespace Assistant
{
    [RequireComponent(typeof(AmmoTriggers))]
    [RequireComponent(typeof(BasketAssistant))]
    [RequireComponent(typeof(AssistantStateMachine))]
    [RequireComponent(typeof(IdleState))]
    public class CargoAssistant : MonoCache
    {
        [SerializeField] private BasketAssistant _basket;

        private CartridgeGun[] _cartridgeGuns;
        private IdleState _idleState;
        private AmmoTriggers _ammoTriggers;
        private StorageAmmoPlate _storageAmmoPlate;

        public AssistantData AssistantData { get; private set; }

        public void Construct(AssistantData assistantData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate, PoolAmmoBoxAssistant pool)
        {
            _storageAmmoPlate = storageAmmoPlate;
            _basket.Construct(pool, assistantData.SizeBasket);

            AssistantData = assistantData;
            _cartridgeGuns = cartridgeGuns;

            
            _idleState.Inject(_storageAmmoPlate.transform.position, _basket);

            foreach (CartridgeGun cartridgeGun in _cartridgeGuns)
                cartridgeGun.Activated += UpdateListPoints;
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

            foreach (CartridgeGun cartridgeGun in _cartridgeGuns)
                cartridgeGun.Activated -= UpdateListPoints;
        }

        private void OnValidate()
        {
            _idleState = Get<IdleState>();
            _ammoTriggers = Get<AmmoTriggers>();
        }


        private void OnStorageEntered() =>
            _basket.Replenishment().Forget();

        private void OnStorageExited() =>
            _basket.StopReplenishment();

        private void OnCartridgeGunEntered(CartridgeGun cartridgeGun)
        {
            if (_basket.IsEmpty)
                return;

            cartridgeGun.SetPresenceCourier(false);
            cartridgeGun.ApplyBox(_basket);
        }

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) =>
            cartridgeGun.SetPresenceCourier(true);

        public void UpdateListPoints()
        {
            _idleState.SetActualPoint(_cartridgeGuns.Where(gun =>
                gun.isActiveAndEnabled).ToList());
        }
    }
}