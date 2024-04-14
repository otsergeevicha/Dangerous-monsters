using Enemies;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Ammo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoCache
    {
        [HideInInspector] [SerializeField] private Rigidbody _rigidbody;
        private Vector3 _firstPosition;
        private BulletData _bulletData;

        private void OnValidate() => 
            _rigidbody ??= Get<Rigidbody>();

        public void Construct(BulletData bulletData) => 
            _bulletData = bulletData;

        private void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(_bulletData.BulletDamage);

            gameObject.SetActive(false);
        }

        public void InActive() => 
            gameObject.SetActive(false);

        public void Shot(Vector3 currentPosition, Vector3 direction)
        {
            transform.position = currentPosition;
            transform.LookAt(direction);
            gameObject.SetActive(true);
            
            _rigidbody.velocity = transform.forward * _bulletData.BulletSpeed;
        }
    }
}