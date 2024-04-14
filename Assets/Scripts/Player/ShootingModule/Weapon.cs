using System.Linq;
using System.Threading;
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
        
        private const int DelayShots = 100;
        
        private readonly CancellationTokenSource _shootToken = new ();

        private IMagazine _magazine;
        private PoolBullet _pool;
        private Enemy _currentTarget;
        private bool _isAttack;

        public void Construct(PoolBullet poolBullet, IMagazine magazine)
        {
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
                    _pool.Bullets.FirstOrDefault(bullet =>
                            bullet.isActiveAndEnabled == false)
                        ?.Shot(_spawnPoint.position, _currentTarget.transform.position);

                    _magazine.Spend();
                }

                _magazine.Shortage();

                await UniTask.Delay(DelayShots);
            }
        }
    }
}