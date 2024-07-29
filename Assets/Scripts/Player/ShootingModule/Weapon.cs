using System.Linq;
using System.Threading;
using CameraModule;
using Cysharp.Threading.Tasks;
using Enemies;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using UnityEngine;

namespace Player.ShootingModule
{
    public class Weapon : MonoCache
    {
        [SerializeField] private Transform _spawnPoint;

        private const int DelayShots = 500;

        private readonly CancellationTokenSource _shootToken = new();

        private IMagazine _magazine;
        private PoolBullet _pool;
        private Enemy _currentTarget;
        private bool _isAttack;
        private CameraFollow _camera;
        private AudioSource _audioSource;

        public void Construct(PoolBullet poolBullet, IMagazine magazine, CameraFollow cameraFollow,
            AudioSource audioSource)
        {
            _audioSource = audioSource;
            _camera = cameraFollow;
            _magazine = magazine;
            _pool = poolBullet;
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
                    
                    _pool.Bullets.FirstOrDefault(bullet =>
                            bullet.isActiveAndEnabled == false)
                        ?.Shot(_spawnPoint.position, _currentTarget.transform.position);

                   // _camera.Shake();
                    _magazine.Spend();
                }

                _magazine.Shortage();

                await UniTask.Delay(DelayShots);
            }
        }
    }
}