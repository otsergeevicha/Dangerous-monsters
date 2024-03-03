using Ammo;
using ContactPlatforms;
using Plugins.MonoCache;
using SO;
using Turret;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AssistantAnimation))]
    [RequireComponent(typeof(AmmoTriggers))]
    public class CargoAssistant : MonoCache
    {
        [HideInInspector] [SerializeField] private AmmoTriggers _ammoTriggers;
        
        public BasketCargoAssistant Basket;
        
        public AssistantAnimation AssistantAnimation{ get; private set; }
        public AssistantData AssistantData { get; private set; }
        public StorageAmmoPlate StorageAmmoPlate { get; private set; }
        public CartridgeGun[] CartridgeGuns { get; private set; }

        public int GetMaxSizeBasket =>
            AssistantData.SizeBasket;

        public void Construct(AssistantData assistantData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate)
        {
            AssistantData = assistantData;
            StorageAmmoPlate = storageAmmoPlate;
            CartridgeGuns = cartridgeGuns;

            AssistantAnimation = Get<AssistantAnimation>();
            AssistantAnimation.Construct(assistantData);
        }

        private void OnValidate() => 
            _ammoTriggers = Get<AmmoTriggers>();

        private void Start()
        {
            print("все здесь временно для тестов, должно все быть в методе OnActive. Сейчас для тестов в старте");
            _ammoTriggers.StorageEntered += OnStorageEntered;
            _ammoTriggers.StorageExited += OnStorageExited;
            
            _ammoTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited += OnCartridgeGunExited;
        }

        public void OnActive()
        {
            _ammoTriggers.StorageEntered += OnStorageEntered;
            _ammoTriggers.StorageExited += OnStorageExited;
            
             _ammoTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
             _ammoTriggers.CartridgeGunExited += OnCartridgeGunExited;
        }

        private void OnStorageEntered() => 
            Basket.Replenishment().Forget();
        
        private void OnStorageExited() =>
            Basket.StopReplenishment();
        
        private void OnCartridgeGunEntered(CartridgeGun cartridgeGun)
        {
            if (Basket.IsEmpty)
                return;

            cartridgeGun.SetPresenceCourier(false);
            cartridgeGun.ApplyBox(Basket);
        }

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) => 
            cartridgeGun.SetPresenceCourier(true);

        public void InActive()
        {
            _ammoTriggers.StorageEntered -= OnStorageEntered;
            _ammoTriggers.StorageExited -= OnStorageExited;
            
             _ammoTriggers.CartridgeGunEntered -= OnCartridgeGunEntered;
             _ammoTriggers.CartridgeGunExited -= OnCartridgeGunExited;
            gameObject.SetActive(false);
        }
    }
}