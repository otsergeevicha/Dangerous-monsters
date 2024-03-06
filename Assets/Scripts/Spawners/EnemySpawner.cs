using System.Linq;
using Cysharp.Threading.Tasks;
using Enemies;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Spawners
{
    public class EnemySpawner : MonoCache
    {
        private Transform[] _squarePoints;
        private PoolEnemies _pool;
        private EnemySpawnerData _enemySpawnerData;
        private bool _isWork = true;

        public void Construct(Transform[] squareEnemySpawner, PoolEnemies pool, EnemySpawnerData enemySpawnerData)
        {
            _enemySpawnerData = enemySpawnerData;
            _pool = pool;
            _squarePoints = squareEnemySpawner;

            foreach (Enemy enemy in _pool.Enemies) 
                enemy.Died += ReuseEnemy;
        }

        private void Start()
        {
            LaunchSpawn().Forget();
        }

        protected override void OnDisabled()
        {
            foreach (Enemy enemy in _pool.Enemies) 
                enemy.Died -= ReuseEnemy;
        }

        private void ReuseEnemy()
        {
            Enemy currentEnemy = _pool.Enemies.FirstOrDefault(enemy =>
                enemy.isActiveAndEnabled == false);

            if (currentEnemy != null)
            {
                currentEnemy.transform.position = GetRandomPoint();
                currentEnemy.OnActive();
            }
            else
            {
                _isWork = false;
            }
        }

        private async UniTaskVoid LaunchSpawn()
        {
            while (_isWork)
            {
                ReuseEnemy();
                
                await UniTask.Delay(_enemySpawnerData.IntervalSpawn);
            }
        }

        private Vector3 GetRandomPoint()
        {
            float randomX = Random.Range(Mathf.Min(_squarePoints[0].position.x, _squarePoints[1].position.x),
                Mathf.Max(_squarePoints[2].position.x, _squarePoints[3].position.x));

            float randomZ = Random.Range(Mathf.Min(_squarePoints[0].position.z, _squarePoints[2].position.z),
                Mathf.Max(_squarePoints[1].position.z, _squarePoints[3].position.z));

            return new Vector3(randomX, 0f, randomZ);
        }
    }
}