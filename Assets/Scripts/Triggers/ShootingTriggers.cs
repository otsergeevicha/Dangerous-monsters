using System;
using Enemies;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class ShootingTriggers : MonoCache
    {
        public event Action OnZone;
        public Enemy CurrentEnemy { get; private set; }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                CurrentEnemy = enemy;
                OnZone?.Invoke();
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy) == CurrentEnemy) 
                CurrentEnemy = null;
        }
    }
}