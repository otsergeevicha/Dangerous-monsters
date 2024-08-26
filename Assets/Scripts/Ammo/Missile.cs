using Enemies;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Ammo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Missile : MonoCache
    {
        private BulletData _bulletData;
        private Collider[] _overlappedColliders;
        private Vector3 _targetPosition;
        private Vector3 _moveDirection;
        private bool _isActive;
        private float _distanceBefore;
        private float _speed;
        private EffectModule _effectModule;

        public void Construct(BulletData bulletData, EffectModule effectModule)
        {
            _effectModule = effectModule;
            _bulletData = bulletData;
            _speed = _bulletData.MissileSpeed;
        }

        protected override void FixedUpdateCached()
        {
            if (!_isActive)
                return;
            
            transform.position += _moveDirection * (Time.fixedDeltaTime * _speed);
            
            if (_distanceBefore <= Vector3.Distance(transform.position, _targetPosition)) 
                InActive();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy _))
            {
                _overlappedColliders = Physics.OverlapSphere(transform.position, _bulletData.RadiusExplosion);

                int count = _overlappedColliders.Length;
            
                for (int i = 0; i < count; i++)
                {
                    if (_overlappedColliders[i].gameObject.TryGetComponent(out Enemy enemy))
                        enemy.ApplyDamage(_bulletData.MissileDamage);
                }
            
                _effectModule.OnExplosion(transform.position);
                InActive();
            }
        }

        public void InActive()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }

        public void Throw(Vector3 currentPosition, Vector3 targetPosition)
        {
            transform.position = currentPosition;
            
            _targetPosition = targetPosition;
            _distanceBefore = Vector3.Distance(currentPosition, _targetPosition);
            _moveDirection = (_targetPosition - currentPosition).normalized;
            
            gameObject.SetActive(true);
            
            _isActive = true;
        }
    }
}