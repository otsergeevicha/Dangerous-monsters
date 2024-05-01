using System;
using Plugins.MonoCache;
using UnityEngine;

namespace RingZone
{
    public class EnemyRing : MonoCache
    {
        private Transform _followingTransform;

        private void Start() => 
            InActive();

        protected override void UpdateCached()
        {
            if (!isActiveAndEnabled)
                return;

            transform.position = _followingTransform.position;
        }

        public void OnActive(Transform enemyTransform)
        {
            _followingTransform = enemyTransform;
            gameObject.SetActive(true);
        }

        public void InActive()
        {
            gameObject.SetActive(false);
            _followingTransform = null;
        }
    }
}