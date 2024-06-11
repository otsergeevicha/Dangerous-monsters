using System;
using Enemies.AI;
using HpBar;
using Loots;
using SO;
using Spawners;
using UnityEngine;

namespace Enemies
{
    public class SixEnemy : Enemy
    {
        private DirectionOperator _directionOperator;
        private HealthOperator _healthOperator;

        private int _maxHealth;
        private int _currentHealth;
        private LootSpawner _lootSpawner;
        private HealthBar _healthBar;
        public override event Action Died;
        public override bool IsCalm { get; protected set; } = true;
        public override bool IsReached { get; protected set; }
        private EnemyData EnemyData { get; set; }

        public override int GetId() =>
            (int)EnemyId.SixLevel;

        public override void Construct(EnemyData enemyData, DirectionOperator directionOperator,
            HealthOperator healthOperator, LootSpawner lootSpawner, HealthBar healthBar)
        {
            _healthBar = healthBar;
            _lootSpawner = lootSpawner;
            _healthOperator = healthOperator;
            _directionOperator = directionOperator;
            EnemyData = enemyData;
            _maxHealth = EnemyData.SixLevelHealth;
            
            ResetHealth();
        }

        public override void OnActive() =>
            gameObject.SetActive(true);

        public override void InActive()
        {
            gameObject.SetActive(false);
            ResetHealth();
        }
        
        public override Vector3 GetDirection() =>
            _directionOperator.Generate(transform.position, Vector3.zero, EnemyData.DeviationAmount);

        public override void SetReached(bool flag) =>
            IsReached = flag;
        
        public override void TakeDamage(int damage)
        {
            _currentHealth = _healthOperator.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                SpawnLoot();
                InActive();
                Died?.Invoke();
            }
        }

        private void SpawnLoot() => 
            _lootSpawner.Spawn(GetId(), transform.position);

        private void ResetHealth() =>
            _currentHealth = _maxHealth;
    }
}