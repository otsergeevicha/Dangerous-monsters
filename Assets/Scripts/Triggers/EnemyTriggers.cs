using System;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class EnemyTriggers : MonoCache
    {
        public event Action OnAgro;
        public event Action NonAgro;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero _)) 
                OnAgro?.Invoke();
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero _)) 
                NonAgro?.Invoke();
        }
    }
}