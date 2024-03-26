using Enemies;
using Plugins.MonoCache;
using SO;
using Unity.Mathematics;
using UnityEngine;

namespace Ammo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Missile : MonoCache
    {
        [HideInInspector] [SerializeField] private Rigidbody _rigidbody;
        
        private BulletData _bulletData;
        private Collider[] _overlappedColliders;

        public void Construct(BulletData bulletData) => 
            _bulletData = bulletData;

        private void OnValidate() => 
            _rigidbody = Get<Rigidbody>();

        private void OnTriggerEnter(Collider collision) => 
            Explosion();

        private void Explosion()
        {
            _overlappedColliders = Physics.OverlapSphere(transform.position, _bulletData.RadiusExplosion);

            int count = _overlappedColliders.Length;
            
            for (int i = 0; i < count; i++)
            {
                if (_overlappedColliders[i].gameObject.TryGetComponent(out Enemy enemy))
                    enemy.TakeDamage(_bulletData.MissileDamage);
            }
            
            InActive();
        }

        public void OnActive() => 
            gameObject.SetActive(true);
        
        public void InActive() => 
            gameObject.SetActive(false);

        public void SetStartPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            transform.rotation = quaternion.identity;
        }

        public void Throw(Vector3 forward)
        {
            transform.forward = forward;
            _rigidbody.velocity = forward;
        }
    }
}