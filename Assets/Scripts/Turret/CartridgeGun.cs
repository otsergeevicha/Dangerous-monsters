using System;
using Assistant;
using Cysharp.Threading.Tasks;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Turret
{
    public class CartridgeGun : MonoCache
    {
        [SerializeField] private CartridgeBox[] _cartridgeBoxes;

        private const int MillisecondsDelay = 500;
        private bool _isCourierExited;

        public event Action Activated;
        public event Action<Vector3> DownloadRequired;
        public event Action<Vector3> Fulled;
        public bool IsRequiredDownload { get; private set; } = true;

        private void Start()
        {
            if (_cartridgeBoxes.Length != 0)
            {
                for (int i = 0; i < _cartridgeBoxes.Length; i++)
                    _cartridgeBoxes[i].InActive();
            }
        }

        public void OnActive() => 
            Activated?.Invoke();

        public void SetPresenceCourier(bool status) =>
            _isCourierExited = status;

        public void ApplyBox(BasketPlayer basketPlayer)
        {
            if (_cartridgeBoxes.Length == 0)
                return;

            ReplenishmentHero(basketPlayer).Forget();
        }
        
        public void ApplyBox(BasketCargoAssistant basketCargoAssistant)
        {
            if (_cartridgeBoxes.Length == 0)
                return;

            ReplenishmentHero(basketCargoAssistant).Forget();
        }

        private async UniTaskVoid ReplenishmentHero(BasketCargoAssistant basketCargoAssistant)
        {
            for (int i = 0; i < _cartridgeBoxes.Length; i++)
            {
                if (_isCourierExited)
                    return;

                if (basketCargoAssistant.IsEmpty)
                    return;

                if (_cartridgeBoxes[i].isActiveAndEnabled == false)
                {
                    _cartridgeBoxes[i].OnActive();
                    basketCargoAssistant.SpendBox();
                    
                    await UniTask.Delay(MillisecondsDelay);
                }
            }
        }
        
        private async UniTaskVoid ReplenishmentHero(BasketPlayer basketPlayer)
        {
            for (int i = 0; i < _cartridgeBoxes.Length; i++)
            {
                if (_isCourierExited)
                    return;

                if (basketPlayer.IsEmpty)
                    return;

                if (_cartridgeBoxes[i].isActiveAndEnabled == false)
                {
                    _cartridgeBoxes[i].OnActive();
                    basketPlayer.SpendBox();
                    
                    await UniTask.Delay(MillisecondsDelay);
                }
            }
        }
    }
}