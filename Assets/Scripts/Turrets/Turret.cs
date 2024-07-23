using System.Collections;
using System.Linq;
using Ammo;
using Enemies;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using Services.Bank;
using SO;
using Triggers;
using Turrets.Children;
using UnityEngine;

namespace Turrets
{
    public class Turret : MonoCache
    {
        [SerializeField] private TurretTrigger _trigger;
        [SerializeField] private Transform _spawnPointGrenade;

        private Collider[] _overlappedColliders = new Collider[5];
        private TurretData _turretData;
        private PoolMissiles _poolMissiles;
        private CartridgeGun _cartridgeGun;
        private Transform _turretBody;
        private Coroutine _coroutine;
        
        private int _price;

        public void Construct(CartridgeGun cartridgeGun, TurretData turretData, PoolMissiles poolMissiles)
        {

            _cartridgeGun = cartridgeGun;
            _poolMissiles = poolMissiles;
            _turretData = turretData;
            _turretBody = transform;
        }

        public void OnActive(Transform spawnPoint, int currentPrice)
        {
            _price = currentPrice;
            
            gameObject.SetActive(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            transform.position = spawnPoint.position;

            _trigger.OnActiveCollider();
            _trigger.SetRadiusTrigger(_turretData.RadiusDetection);
            _trigger.Invasion += OnAttack;
        }

        public void InActive()
        {
            _trigger.InActiveCollider();
            _trigger.Invasion -= OnAttack;
            gameObject.SetActive(false);
        }

        public void Upgrade()
        {
            _turretData.RadiusDetection++;
            _trigger.SetRadiusTrigger(_turretData.RadiusDetection);
        }

        public int GetPrice() => 
            _price;

        public void IncreasePrice(int stepIncreasePrice) => 
            _price += stepIncreasePrice;

        private void OnAttack()
        {
            _overlappedColliders = Physics.OverlapSphere(transform.position, _turretData.RadiusDetection);

            for (int i = 0; i < _overlappedColliders.Length; i++)
            {
                if (_overlappedColliders[i].gameObject.TryGetComponent(out Enemy enemy))
                {
                    if (_coroutine != null)
                        StopCoroutine(_coroutine);
            
                    _coroutine = StartCoroutine(RotateTurretAndAttack(enemy.transform.position));
                    break;
                }
            }
        }

        private IEnumerator RotateTurretAndAttack(Vector3 enemyPosition)
        {
            Vector3 fromTo = enemyPosition - transform.position;
            fromTo.y = .0f;
            Quaternion lookRotation = Quaternion.LookRotation(fromTo);

            while (_turretBody.rotation != lookRotation)
            {
                _turretBody.rotation = Quaternion.RotateTowards(_turretBody.rotation, lookRotation,
                    _turretData.RotateSpeed * Time.deltaTime);
                yield return null;
            }

            Shoot(enemyPosition);
        }

        private void Shoot(Vector3 targetPosition)
        {
            Missile missile = _poolMissiles.Missiles.FirstOrDefault(bullet =>
                bullet.isActiveAndEnabled == false);

            if (missile != null && _cartridgeGun.CheckMagazine)
            {
                missile.SetStartPosition(_spawnPointGrenade.position);
                missile.Throw(_spawnPointGrenade.position, targetPosition);
                _cartridgeGun.Spend();
            }
            
            OnAttack();
        }
    }
}