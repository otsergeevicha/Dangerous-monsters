using System.Collections.Generic;
using CameraModule;
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
        private List<Enemy> _enemies = new List<Enemy>();
        private EnemyRing _enemyRing;
        private bool _haveTarget;
        private Enemy _currentEnemy;
        private int _requestTarget;

        public void Construct(HeroAnimation heroAnimation, HeroMovement heroMovement,
            WeaponHolder weaponHolder, float heroDataRadiusDetection, EnemyRing enemyRing)
        {
            _enemyRing = enemyRing;
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

        public void SetOnBase(bool heroOnBase)
        {
            _heroOnBase = heroOnBase;
            _weaponHolder.Disarmed(heroOnBase);
        }

        public void MergeEnemies(List<Enemy> poolSimpleEnemies, List<Enemy> poolBosses)
        {
            _enemies = new List<Enemy>();
            
            _enemies.AddRange(poolSimpleEnemies);
            _enemies.AddRange(poolBosses);
            
            OffShoot();
        }

        private void FindNearestEnemy()
        {
            float minDistance = _heroDataRadiusDetection;
            Vector3 heroPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            foreach (var enemy in _enemies)
            {
                float distance = Vector3.Distance(heroPosition, enemy.transform.position);

                if (distance <= _heroDataRadiusDetection && distance < minDistance && enemy.IsDie == false)
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
            _weaponHolder.GetActiveGun()?.OffShoot();
            _heroMovement.SetStateBattle(false, null);
        }

        private void Shoot(Enemy enemy)
        {
            if (!_heroOnBase)
            {
                _heroAnimation.EnableShoot();
                _weaponHolder.GetActiveGun().Shoot(enemy);
                _heroMovement.SetStateBattle(true, enemy.transform);
            }
        }
    }
}