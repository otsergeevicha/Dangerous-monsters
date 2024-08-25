using System.Threading;
using CameraModule;
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
        private const int DelayShots = 500;

        private readonly CancellationTokenSource _shootToken = new();

        private IMagazine _magazine;
        private Enemy _currentTarget;
        private bool _isAttack;
        private CameraFollow _camera;
        private AudioSource _audioSource;
        private BulletData _bulletData;
        private EffectModule _effectModule;

        public void Construct(IMagazine magazine, CameraFollow cameraFollow,
            AudioSource audioSource, BulletData bulletData, EffectModule effectModule)
        {
            _effectModule = effectModule;
            _bulletData = bulletData;
            _audioSource = audioSource;
            _camera = cameraFollow;
            _magazine = magazine;
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

        private async UniTaskVoid ImitationQueue()
        {
            while (_isAttack)
            {
                if (_magazine.Check())
                {
                    _audioSource.Play();
                    
                    _currentTarget.ApplyDamage(_bulletData.BulletDamage);
                    _effectModule.OnHitEnemy(_currentTarget.gameObject.transform.position);

                   // _camera.Shake();
                    _magazine.Spend();
                }

                _magazine.Shortage();

                await UniTask.Delay(DelayShots);
            }
        }
    }
}