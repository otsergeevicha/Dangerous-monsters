using System;
using Enemies.AI;
using Enemies.Animation;
using HpBar;
using Modules;
using Plugins.MonoCache;
using SO;
using Spawners;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    enum EnemyId
    {
        ZeroLevel,
        OneLevel,
        TwoLevel,
        ThreeLevel,
        FourLevel,
        FiveLevel,
        SixLevel,
        SevenLevel,
        EightLevel,
        NineLevel
    }

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimation))]
    public abstract class Enemy : MonoCache
    {
        private HealthBar _healthBar;
        private LootSpawner _lootSpawner;
        private EnemyHealthModule _enemyHealthModule;
        private DirectionOperator _directionOperator;
        private int _maxHealth;
        private EnemyData _enemyData;
        private int _currentHealth;

        public event Action Died;
        public bool IsCalm { get; protected set; } = true;
        public bool IsReached { get; private set; }
        
        protected abstract int GetId();
        public void Construct(EnemyData enemyData, DirectionOperator directionOperator,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, HealthBar healthBar)
        {
            _healthBar = healthBar;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;
            _directionOperator = directionOperator;
            _enemyData = enemyData;
            _maxHealth = enemyData.EightLevelHealth;
            
            ResetHealth();
        }

        public Vector3 GetDirection() => 
            _directionOperator.Generate(transform.position, Vector3.zero, _enemyData.DeviationAmount);

        public void SetReached(bool flag) =>
            IsReached = flag;
        
        public void ApplyDamage(int damage)
        {
            _currentHealth = _enemyHealthModule.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, _maxHealth);
            
            if (_currentHealth <= 0)
            {
                SpawnLoot();
                InActive();
                Died?.Invoke();
            }
        }
        
        public virtual void OnActive() => 
            gameObject.SetActive(true);

        public virtual void InActive()
        {
            gameObject.SetActive(false);
            ResetHealth();
        }

        private void SpawnLoot() => 
            _lootSpawner.Spawn(GetId(), transform.position);
        
        private void ResetHealth() => 
            _currentHealth = _maxHealth;
    }
}