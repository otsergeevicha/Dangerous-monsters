﻿using System;
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

        public void ApplyBox(AmmoBasket ammoBasket)
        {
            if (_cartridgeBoxes.Length == 0)
                return;

            Replenishment(ammoBasket).Forget();
        }

        public void SetPresenceCourier(bool status) =>
            _isCourierExited = status;

        private async UniTaskVoid Replenishment(AmmoBasket ammoBasket)
        {
            for (int i = 0; i < _cartridgeBoxes.Length; i++)
            {
                if (_isCourierExited)
                    return;

                if (ammoBasket.IsEmpty)
                    return;

                if (_cartridgeBoxes[i].isActiveAndEnabled == false)
                {
                    _cartridgeBoxes[i].OnActive();
                    ammoBasket.SpendBox();
                    
                    await UniTask.Delay(MillisecondsDelay);
                }
            }
        }
    }
}