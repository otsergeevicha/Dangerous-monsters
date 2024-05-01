using System;
using Enemies;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class ShootingTriggers : MonoCache
    {
        public event Action OnZone;
        public event Action Exited;
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
            {
                Exited?.Invoke();
                CurrentEnemy = null;
            }
        }
    }
}