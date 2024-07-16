using System;
using Plugins.MonoCache;
using UnityEngine;

namespace Canvases.UpgradePlayer
{
    public class UpgradePlayerBoard : MonoCache
    {
        public event Action OnEntered;
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Player.Hero _)) 
                OnEntered?.Invoke();
        }
    }
}