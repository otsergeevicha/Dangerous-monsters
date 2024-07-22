using Enemies;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Ammo
{
    public class Bullet : MonoCache
    {
        private Vector3 _firstPosition;
        private BulletData _bulletData;
        private EffectModule _effectModule;
        private bool _isActive;
        private float _distanceBefore;
        private int _speed;
        private Vector3 _moveDirection;
        private Vector3 _targetPosition;

        public void Construct(BulletData bulletData, EffectModule effectModule)
        {
            _effectModule = effectModule;
            _bulletData = bulletData;
            _speed = _bulletData.BulletSpeed;
        }

        protected override void UpdateCached()
        {
            if (!_isActive)
                return;
            
            transform.position += _moveDirection * (Time.deltaTime * _speed);

            if (_distanceBefore <= Vector3.Distance(transform.position, _targetPosition)) 
                InActive();
        }

        public void InActive()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }

        public void Shot(Vector3 currentPosition, Vector3 targetPosition)
        {
            _targetPosition = new Vector3(targetPosition.x, targetPosition.y + 1, targetPosition.z);
            
            transform.position = currentPosition;
             _distanceBefore = Vector3.Distance(currentPosition, _targetPosition);
             _moveDirection = (_targetPosition - currentPosition).normalized;
             
            gameObject.SetActive(true);

             _isActive = true;
        }

        private void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyDamage(_bulletData.BulletDamage);
                _effectModule.OnHitEnemy(new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z));
            }
        }
    }
}