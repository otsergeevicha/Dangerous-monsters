using System;
using System.Linq;
using Ammo;
using Enemies;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using SO;
using Turrets.Children;
using UnityEngine;

namespace Turrets
{
    public class Turret : MonoCache
    {
        [SerializeField] private TurretTrigger _trigger;
        [SerializeField] private Transform _spawnPointGrenade;
        
        private readonly float _ourGravity = Physics.gravity.y;
        
        private Collider[] _overlappedColliders = new Collider[5];
        private TurretData _turretData;
        private PoolMissiles _poolMissiles;
        private CartridgeGun _cartridgeGun;

        public void Construct(CartridgeGun cartridgeGun, TurretData turretData, PoolMissiles poolMissiles)
        {
            _cartridgeGun = cartridgeGun;
            _poolMissiles = poolMissiles;
            _turretData = turretData;
        }

        private void Start()
        {
            print("все здесь временно для тестов, должно все быть в методе OnActive. Сейчас для тестов в старте");

            _trigger.OnActiveCollider();
            _trigger.SetRadiusTrigger(_turretData.RadiusDetection);
            _trigger.Invasion += OnAttack;
        }

        public void OnActive()
        {
            _trigger.OnActiveCollider();
            _trigger.SetRadiusTrigger(_turretData.RadiusDetection);
            _trigger.Invasion += OnAttack;
            gameObject.SetActive(true);
        }

        public void InActive()
        {
            _trigger.InActiveCollider();
            _trigger.Invasion -= OnAttack;
            gameObject.SetActive(false);
        }

        private void OnAttack()
        {
            _overlappedColliders = Physics.OverlapSphere(transform.position, _turretData.RadiusDetection);

            for (int i = 0; i < _overlappedColliders.Length; i++)
            {
                if (_overlappedColliders[i].gameObject.TryGetComponent(out Enemy enemy))
                {
                    Vector3 fromTo = enemy.transform.position - transform.position;
                    Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

                    float axisX = fromToXZ.magnitude;
                    float axisY = fromTo.y;

            
                    float angleInRadians = _turretData.AngleInDegrees * MathF.PI / 180;
                    float rootOfSpeed = (_ourGravity * axisX * axisX) / (2 * (axisY - Mathf.Tan(angleInRadians) * axisX) *
                                                                           Mathf.Pow(Mathf.Cos(angleInRadians), 2));
                    float speed = Mathf.Sqrt(Mathf.Abs(rootOfSpeed));

                    Missile missile =
                        _poolMissiles.Missiles.FirstOrDefault(bullet => bullet.isActiveAndEnabled == false);

                    if (missile != null && _cartridgeGun.CheckMagazine)
                    {
                        missile.SetStartPosition(_spawnPointGrenade.position);
                        missile.Throw(_spawnPointGrenade.forward * speed);
                        _cartridgeGun.Spend();
                    }
                    
                    break;
                }
            }
        }
    }
}