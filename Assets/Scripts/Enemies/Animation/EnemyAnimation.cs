using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Enemies.Animation
{
    public class EnemyAnimation : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        
        private EnemyData _enemyData;

        public void Construct(EnemyData enemyData) => 
            _enemyData = enemyData;

        private void OnValidate() => 
            _animator ??= ChildrenGet<Animator>();

        public void EnableIdle()
        {
            throw new System.NotImplementedException();
        }

        public void EnablePursuit()
        {
            throw new System.NotImplementedException();
        }

        public void EnableRun()
        {
            throw new System.NotImplementedException();
        }

        public void EnableAgro()
        {
            throw new System.NotImplementedException();
        }

        public void EnableAttack()
        {
            throw new System.NotImplementedException();
        }

        public void EnableEnrage()
        {
            throw new System.NotImplementedException();
        }

        public void EnableDie()
        {
            throw new System.NotImplementedException();
        }
    }
}