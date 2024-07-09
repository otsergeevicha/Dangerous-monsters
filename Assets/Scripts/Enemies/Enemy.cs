﻿using System;
using System.Linq;
using ContactZones;
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
        private readonly BossId[] _bossLevels = Enum.GetValues(typeof(BossId)).Cast<BossId>().ToArray();

        protected int MaxHealth;

        private HealthBar _healthBar;
        private LootSpawner _lootSpawner;
        private EnemyHealthModule _enemyHealthModule;
        private FinishPlate _finishPlate;

        private int _currentHealth;

        public event Action Died;
        protected EnemyData EnemyData { get; private set; }

        protected abstract int GetId();
        protected abstract void SetCurrentHealth();

        public void Construct(EnemyData enemyData,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, HealthBar healthBar, FinishPlate finishPlate)
        {
            _finishPlate = finishPlate;
            _healthBar = healthBar;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;
            EnemyData = enemyData;

            ResetHealth();
        }

        public void ApplyDamage(int damage)
        {
            _currentHealth = _enemyHealthModule.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, MaxHealth);

            if (_currentHealth <= 0)
            {
                if (_bossLevels.Contains((BossId)GetId()))
                    _finishPlate.OnActive();
                else
                {
                    SpawnLoot();
                    Died?.Invoke();
                }

                InActive();
            }
        }

        public virtual void OnActive() =>
            gameObject.SetActive(true);

        public virtual void InActive()
        {
            gameObject.SetActive(false);
            ResetHealth();
        }

        public void OnDestroy()
        {
            Destroy(_healthBar);
            Destroy(this);
        }

        private void SpawnLoot() =>
            _lootSpawner.SpawnMoney(GetId(), transform.position);

        private void ResetHealth() =>
            _currentHealth = MaxHealth;
    }
}