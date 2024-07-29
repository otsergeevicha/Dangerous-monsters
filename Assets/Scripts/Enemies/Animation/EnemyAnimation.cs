using Plugins.MonoCache;
using SO;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Animation
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoCache
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _attackPoint;

        private const string LayerName = "Player";
        private readonly float _cleavage = 0.5f;

        private Collider[] _hits = new Collider[1];
        private int _layerMask;
        private EnemyData _enemyData;
        private Enemy _enemy;
        private NavMeshAgent _agent;

        public void Construct(EnemyData enemyData, Enemy enemy, NavMeshAgent agent)
        {
            _agent = agent;
            _enemy = enemy;
            _enemyData = enemyData;
            _layerMask = 1 << LayerMask.NameToLayer(LayerName);
        }

        private void OnValidate() =>
            _animator ??= Get<Animator>();

        private void HitHero()
        {
            Physics.OverlapSphereNonAlloc(_attackPoint.position, _cleavage, _hits, _layerMask);

            if (_hits[0] != null) 
                _enemy.TakeDamage();
        }

        private void Death() =>
            _enemy.Death();

        private void EndAttack() =>
            _enemy.AttackCompleted();

        public void EnableIdle()
        {
            _animator.SetBool(_enemyData.WalkHash, false);
            _animator.SetBool(_enemyData.AttackHash, false);
        }

        public void EnableRun()
        {
            _agent.isStopped = false;
            _animator.SetBool(_enemyData.WalkHash, true);
            _animator.SetBool(_enemyData.AttackHash, false);
        }

        public void EnableAgro()
        {
            _animator.SetBool(_enemyData.WalkHash, true);
            _animator.SetBool(_enemyData.AttackHash, false);
        }

        public void EnableAttack()
        {
            _agent.isStopped = true;
            _animator.SetBool(_enemyData.WalkHash, false);
            _animator.SetBool(_enemyData.AttackHash, true);
        }

        public void EnableDie()
        {
            _agent.isStopped = true;
            _animator.SetTrigger(_enemyData.DeathHash);
        }
    }
}