using System;
using Cysharp.Threading.Tasks;
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

        public void ApplyBox(IBasket basket)
        {
            if (_cartridgeBoxes.Length == 0)
                return;

            Replenishment(basket).Forget();
        }

        private async UniTaskVoid Replenishment(IBasket basket)
        {
            for (int i = 0; i < _cartridgeBoxes.Length; i++)
            {
                if (_isCourierExited)
                    return;

                if (basket.IsEmpty)
                    return;

                if (_cartridgeBoxes[i].isActiveAndEnabled == false)
                {
                    _cartridgeBoxes[i].OnActive();
                    basket.SpendBox();
                    
                    await UniTask.Delay(MillisecondsDelay);
                }
            }
        }
    }
}