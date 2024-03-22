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

        public void Construct(BulletData bulletData) => 
            _bulletData = bulletData;

        private void OnValidate() => 
            _rigidbody = Get<Rigidbody>();

        public void OnActive() => 
            gameObject.SetActive(true);
        
        public void InActive() => 
            gameObject.SetActive(false);

        public void SetStartPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            transform.rotation = quaternion.identity;
        }

        public void Throw(Vector3 forward) => 
            _rigidbody.velocity = forward;
    }
}