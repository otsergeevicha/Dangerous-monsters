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
    
    enum BossId
    {
        OneLevel = 10,
        TwoLevel = 20,
        ThreeLevel = 30,
        FourLevel = 40,
        FiveLevel = 50,
        SixLevel = 60,
        SevenLevel = 70,
        EightLevel = 80,
        NineLevel = 90,
        TenLevel = 100
    }

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimation))]
    public abstract class Enemy : MonoCache
    {
        protected int MaxHealth;
        
        private HealthBar _healthBar;
        private LootSpawner _lootSpawner;
        private EnemyHealthModule _enemyHealthModule;
        private DirectionOperator _directionOperator;
        private int _currentHealth;

        public event Action Died;
        public bool IsCalm { get; protected set; } = true;
        public bool IsReached { get; private set; }
        protected EnemyData EnemyData { get; private set; }
        
        protected abstract int GetId();
        protected abstract void SetCurrentHealth();
        public void Construct(EnemyData enemyData, DirectionOperator directionOperator,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, HealthBar healthBar)
        {
            _healthBar = healthBar;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;
            _directionOperator = directionOperator;
            EnemyData = enemyData;
            
            ResetHealth();
        }
        
        public Vector3 GetDirection() => 
            _directionOperator.Generate(transform.position, Vector3.zero, EnemyData.DeviationAmount);

        public void SetReached(bool flag) =>
            IsReached = flag;
        
        public void ApplyDamage(int damage)
        {
            _currentHealth = _enemyHealthModule.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, MaxHealth);
            
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
            _lootSpawner.SpawnMoney(GetId(), transform.position);
        
        private void ResetHealth() => 
            _currentHealth = MaxHealth;
    }
}