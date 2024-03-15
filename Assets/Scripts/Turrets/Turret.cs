using System;
using Plugins.MonoCache;
using Turrets.Childrens;

namespace Turrets
{
    public class Turret : MonoCache
    {
        private CartridgeGun _cartridgeGun;

        public void Construct(CartridgeGun cartridgeGun)
        {
            _cartridgeGun = cartridgeGun;
        }

        private void Start()
        {
            print("все здесь временно для тестов, должно все быть в методе OnActive. Сейчас для тестов в старте");
        }

        public void OnActive()
        {
            throw new NotImplementedException();
        }
        
        public void InActive() => 
            gameObject.SetActive(false);
    }
}