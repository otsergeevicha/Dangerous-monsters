using System.Collections.Generic;
using Enemies;
using Player.Animation;
using Player.ShootingModule;
using Plugins.MonoCache;
using RingZone;
using UnityEngine;

namespace Player
{
    public class HeroShooting : MonoCache
    {
        private HeroAnimation _heroAnimation;
        private WeaponHolder _weaponHolder;
        private HeroMovement _heroMovement;

        private bool _heroOnBase = true;
        private float _heroDataRadiusDetection;
        private List<Enemy> _poolEnemies;
        private EnemyRing _enemyRing;
        private bool _haveTarget;
        private Enemy _currentEnemy;
        private int _requestTarget;

        public void Construct(HeroAnimation heroAnimation, HeroMovement heroMovement,
            WeaponHolder weaponHolder, float heroDataRadiusDetection, List<Enemy> poolEnemies, EnemyRing enemyRing)
        {
            _enemyRing = enemyRing;
            _poolEnemies = poolEnemies;
            _heroDataRadiusDetection = heroDataRadiusDetection;

            _heroMovement = heroMovement;
            _weaponHolder = weaponHolder;
            _heroAnimation = heroAnimation;
        }

        protected override void UpdateCached()
        {
            if (_heroOnBase)
                return;

            FindNearestEnemy();

            if (_requestTarget == 1)
            {
                _enemyRing.OnActive(_currentEnemy.transform);
                Shoot(_currentEnemy);
            }
            
            if (!_haveTarget)
            {
                _requestTarget = 0;
                OffShoot();
                _enemyRing.InActive();
            }
        }

        public void SetOnBase(bool heroOnBase) =>
            _heroOnBase = heroOnBase;

        private void FindNearestEnemy()
        {
            float minDistance = _heroDataRadiusDetection;
            Vector3 heroPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            foreach (var enemy in _poolEnemies)
            {
                Vector3 enemyPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y,
                    enemy.transform.position.z);
                float distance = Vector3.Distance(heroPosition, enemyPosition);

                if (distance <= _heroDataRadiusDetection && distance < minDistance)
                {
                    _currentEnemy = enemy;
                    _requestTarget++;

                    if (_currentEnemy != enemy)
                    {
                        _requestTarget = 1;
                    }
                    
                    _haveTarget = true;
                    return;
                }
                
                _haveTarget = false;
            }
        }

        private void OffShoot()
        {
            _weaponHolder.GetActiveGun().OffShoot();
            _heroMovement.SetStateBattle(false, null);
        }

        private void Shoot(Enemy enemy)
        {
            _heroAnimation.EnableShoot();
            _weaponHolder.GetActiveGun().Shoot(enemy);
            _heroMovement.SetStateBattle(true, enemy.transform);
        }
    }
}