using Enemies;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class EnemyTriggers : MonoCache
    {
        [HideInInspector] [SerializeField] private  SphereCollider _collider;
        
        private Enemy _enemy;

        public void Construct(Enemy enemy) => 
            _enemy = enemy;

        private void OnValidate() =>
            _collider ??= Get<SphereCollider>();

        public void SetAgroZone(float agroDistance)
        {
            // _collider.isTrigger = true;
            // _collider.center = transform.position;
            // _collider.radius = agroDistance;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero _)) 
                _enemy.OnAgro();
        }
    }
}