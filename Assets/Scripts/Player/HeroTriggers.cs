using System;
using Plugins.MonoCache;
using UnityEngine;

namespace Player
{
    public class HeroTriggers : MonoCache
    {
        public Action StorageEntered;
        public Action StorageExited;
        
        public Action<CartridgeGun> CartridgeGunEntered;
        public Action<CartridgeGun> CartridgeGunExited;
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out StorageAmmoBox _)) 
                StorageEntered?.Invoke();
            
            if (collision.TryGetComponent(out CartridgeGun cartridge)) 
                CartridgeGunEntered?.Invoke(cartridge);
        }
        
        private void OnTriggerExit(Collider collision)
        {
            if (collision.TryGetComponent(out StorageAmmoBox _)) 
                StorageExited?.Invoke();
            
            if (collision.TryGetComponent(out CartridgeGun cartridge)) 
                CartridgeGunExited?.Invoke(cartridge);
        }
    }
}