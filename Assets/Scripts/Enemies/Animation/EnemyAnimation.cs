using System;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Enemies.Animation
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoCache
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _attackPoint;

        private const string LayerName = "Hero";
        private readonly float _cleavage = 0.5f;
        
        private Collider[] _hits = new Collider[1];
        private int _layerMask;
        private EnemyData _enemyData;

        public event Action IsDamaged;
        public event Action AttackEnded;
        public event Action DiedComplete;

        public void Construct(EnemyData enemyData)
        {
            _enemyData = enemyData;
            _layerMask = 1 << LayerMask.NameToLayer(LayerName);
        }

        private void OnValidate() =>
            _animator ??= Get<Animator>();

        private void HitHero()
        {
            print("атака");
            Physics.OverlapSphereNonAlloc(_attackPoint.position, _cleavage, _hits, _layerMask);

            if (_hits[0] != null) 
                IsDamaged?.Invoke();
        }

        private void EndAttack()
        {
            print("конец атаки");
            AttackEnded?.Invoke();
        }

        private void Death()
        {
            print("дошли");
            DiedComplete?.Invoke();
        }

        public void EnableIdle()
        {
            _animator.SetBool(_enemyData.WalkHash, false);
            _animator.SetBool(_enemyData.AttackHash, false);
        }

        public void EnablePursuit()
        {
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
            _animator.SetBool(_enemyData.WalkHash, true);
            _animator.SetBool(_enemyData.AttackHash, false);
        }

        public void EnableDie() => 
            _animator.SetTrigger(_enemyData.DeathHash);
        
        public void EnableHit() => 
            _animator.SetTrigger(_enemyData.HitHash);
    }
}