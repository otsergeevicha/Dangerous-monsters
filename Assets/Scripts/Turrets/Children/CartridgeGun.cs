using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Plugins.MonoCache;
using Services.Basket;
using UnityEngine;

namespace Turrets.Children
{
    public class CartridgeGun : MonoCache
    {
        [SerializeField] private CartridgeBox[] _cartridgeBoxes;

        private const int MillisecondsDelay = 500;
        private bool _isCourierExited;
        private int _currentAmountBoxes = 0;
        private int _maxAmount;

        public event Action Activated;

        public bool IsRequiredDownload =>
            _currentAmountBoxes >= 0 && _currentAmountBoxes != _maxAmount;

        public bool CheckMagazine =>
            _currentAmountBoxes > 0;

        private void Start()
        {
            _maxAmount = _cartridgeBoxes.Length;

            if (_maxAmount != 0)
            {
                for (int i = 0; i < _maxAmount; i++)
                    _cartridgeBoxes[i].InActive();
            }
        }

        public void OnActive() =>
            Activated?.Invoke();

        public void SetPresenceCourier(bool status) =>
            _isCourierExited = status;

        public void ApplyBox(IBasket basket)
        {
            if (_maxAmount == 0)
                return;

            Replenishment(basket).Forget();
        }

        public void Spend()
        {
            _cartridgeBoxes.LastOrDefault(box => box.isActiveAndEnabled)?.InActive();
            _currentAmountBoxes--;
        }

        private async UniTaskVoid Replenishment(IBasket basket)
        {
            for (int i = 0; i < _maxAmount; i++)
            {
                if (_isCourierExited)
                    return;

                if (basket.IsEmpty)
                    return;

                if (_cartridgeBoxes[i].isActiveAndEnabled == false)
                {
                    _cartridgeBoxes[i].OnActive();
                    _currentAmountBoxes++;
                    basket.SpendBox();

                    await UniTask.Delay(MillisecondsDelay);
                }
            }
        }
    }
}