using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ammo;
using CameraModule;
using Canvases;
using Cysharp.Threading.Tasks;
using Enemies;
using Modules;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Player.ShootingModule
{
    public class Weapon : MonoCache
    {
        [Range(1, 100)] [SerializeField] private int _countBullet;
        [SerializeField] private Transform _spawnPointBullet;

        private const int DelayShots = 500;

        private readonly CancellationTokenSource _shootToken = new();

        private IMagazine _magazine;
        private Enemy _currentTarget;
        private bool _isAttack;
        private CameraFollow _camera;
        private AudioSource _audioSource;
        private BulletData _bulletData;
        private EffectModule _effectModule;
        private IReadOnlyList<Bullet> _poolBullets;

        public void Construct(CameraFollow cameraFollow,
            AudioSource audioSource, BulletData bulletData, EffectModule effectModule, Hud hud,
            IReadOnlyList<Bullet> poolBullets)
        {
            _poolBullets = poolBullets;
            _effectModule = effectModule;
            _bulletData = bulletData;
            _audioSource = audioSource;
            _camera = cameraFollow;
            _magazine = new MagazineBullets(_countBullet, hud);
        }

        public void Shoot(Enemy enemy)
        {
            _isAttack = true;
            _currentTarget = enemy;

            _currentTarget.Died += OffShoot;

            ImitationQueue().Forget();
        }

        public void OffShoot()
        {
            _isAttack = false;
            _shootToken.Cancel();
        }

        public void UpdateLevel() =>
            _magazine.UpdateLevel();

        private async UniTaskVoid ImitationQueue()
        {
            while (_isAttack)
            {
                if (_magazine.Check())
                {
                    _audioSource.Play();

                    _poolBullets.FirstOrDefault(bullet => bullet.isActiveAndEnabled == false)
                        ?.Shot(_spawnPointBullet.position,
                        new Vector3(_currentTarget.transform.position.x, 1f, _currentTarget.transform.position.z));

                    //_currentTarget.ApplyDamage(_bulletData.BulletDamage);
                    //_effectModule.OnHitEnemy(_currentTarget.gameObject.transform.position);

                    // _camera.Shake();
                    _magazine.Spend();
                }

                _magazine.Shortage();

                await UniTask.Delay(DelayShots);
            }
        }
    }
}