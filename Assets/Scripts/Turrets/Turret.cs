using System.Collections;
using System.Linq;
using Ammo;
using Enemies;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
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

        public void Construct(CartridgeGun cartridgeGun, TurretData turretData, PoolMissiles poolMissiles)
        {
            _cartridgeGun = cartridgeGun;
            _poolMissiles = poolMissiles;
            _turretData = turretData;
            _turretBody = transform;
        }

        public void OnActive(Transform spawnPoint)
        {
            gameObject.SetActive(true);

            ResetCoroutine();
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
            print("тут логика апгрейда турели");
        }

        private void OnAttack()
        {
            _overlappedColliders = Physics.OverlapSphere(transform.position, _turretData.RadiusDetection);

            for (int i = 0; i < _overlappedColliders.Length; i++)
            {
                if (_overlappedColliders[i].gameObject.TryGetComponent(out Enemy enemy))
                {
                    ResetCoroutine();

                    _coroutine = StartCoroutine(RotateTurretAndAttack(enemy));
                    break;
                }
            }
        }

        private void ResetCoroutine()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator RotateTurretAndAttack(Enemy enemy)
        {
            Vector3 fromTo = enemy.transform.position - transform.position;
            fromTo.y = .0f;
            Quaternion lookRotation = Quaternion.LookRotation(fromTo);

            while (_turretBody.rotation != lookRotation)
            {
                _turretBody.rotation = Quaternion.RotateTowards(_turretBody.rotation, lookRotation,
                    _turretData.RotateSpeed * Time.deltaTime);
                yield return null;
            }

            float axisX = fromTo.x;
            float axisY = fromTo.y;

            Shoot(12f);
        }

        private void Shoot(float speed)
        {
            Missile missile = _poolMissiles.Missiles.FirstOrDefault(bullet =>
                bullet.isActiveAndEnabled == false);

            if (missile != null && _cartridgeGun.CheckMagazine)
            {
                missile.SetStartPosition(_spawnPointGrenade.position);
                missile.OnActive();
                missile.Throw(_spawnPointGrenade.forward * speed);
                _cartridgeGun.Spend();
            }
        }
    }
}