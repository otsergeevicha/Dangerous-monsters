using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Enemies.Animation
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        
        private EnemyData _enemyData;

        public void Construct(EnemyData enemyData) => 
            _enemyData = enemyData;

        private void OnValidate() => 
            _animator = Get<Animator>();
        
        public void EnableRun()
        {
            _animator.SetBool(_enemyData.IdleHash, false);
            _animator.SetBool(_enemyData.RunHash, true);
        }
        
        public void EnableIdle()
        {
            _animator.SetBool(_enemyData.RunHash, false);
            _animator.SetBool(_enemyData.IdleHash, true);
        }
    }
}