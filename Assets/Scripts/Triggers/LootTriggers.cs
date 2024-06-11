using System;
using Bank;
using Loots;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class LootTriggers : MonoCache
    {
        public event Action<int> OnPickUpMoney;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Money money))
            {
                OnPickUpMoney?.Invoke(money.CurrentNominal);
                money.InActive();
            }
        }
    }
}