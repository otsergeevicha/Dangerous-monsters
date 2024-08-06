﻿using System;
using System.Collections;
using System.Linq;
using BehaviorDesigner.Runtime;
using ContactZones;
using Enemies.Animation;
using HpBar;
using Modules;
using Player;
using Plugins.MonoCache;
using SO;
using Spawners;
using Triggers;
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
    public abstract class Enemy : MonoCache
    {
        [SerializeField] private EnemyTriggers _enemyTriggers;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private BehaviorTree _tree;

        private readonly BossId[] _bossLevels = Enum.GetValues(typeof(BossId)).Cast<BossId>().ToArray();
        
        [HideInInspector] public EnemyAnimation EnemyAnimation;
        [HideInInspector] public Transform CashTransform;

        protected int MaxHealth;
        protected int Damage;

        private HealthBar _healthBar;
        private LootSpawner _lootSpawner;
        private EnemyHealthModule _enemyHealthModule;
        private FinishPlate _finishPlate;

        private int _currentHealth;
        private bool _onMap;

        private Hero _hero;
        private Coroutine _coroutine;
        private Vector3 _baseGate;

        public bool IsAgro { get; private set; }
        public bool IsDie { get; private set; }
        public EnemyData EnemyData { get; private set; }
        public Vector3 GetCurrentTarget { get; private set; }


        protected abstract int GetId();
        protected abstract void SetCurrentHealth();
        protected abstract void SetCurrentDamage();
        public event Action Died;

        public void Construct(EnemyData enemyData,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, HealthBar healthBar, FinishPlate finishPlate,
            Hero hero, Vector3 baseGate)
        {
            _enemyTriggers.SetRadius(enemyData.AgroDistance);
            
            CashTransform = transform;
            _tree.enabled = false;

            _baseGate = baseGate;
            _hero = hero;
            _finishPlate = finishPlate;
            _healthBar = healthBar;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;
            EnemyData = enemyData;

            SetCurrentHealth();
            SetCurrentDamage();

            EnemyAnimation.Construct(enemyData, this, _agent);

            _enemyTriggers.OnAgro += () =>
            {
                IsAgro = true;
                GetCurrentTarget = _hero.transform.position;
                _tree.enabled = false;
                _tree.enabled = true;
            };

            _enemyTriggers.NonAgro += () =>
            {
                IsAgro = false;
                GetCurrentTarget = baseGate;
                _tree.enabled = false;
                _tree.enabled = true;
            };

            ResetHealth();
        }

        public void Escape()
        {
            _healthBar.InActive();
            Died?.Invoke();
            IsDie = true;
            _agent.isStopped = true;
            InActive();
        }

        private void OnValidate()
        {
            EnemyAnimation ??= ChildrenGet<EnemyAnimation>();
            _enemyTriggers ??= ChildrenGet<EnemyTriggers>();
            _agent ??= Get<NavMeshAgent>();
            _tree ??= Get<BehaviorTree>();
        }

        public virtual void OnActive()
        {
            IsDie = false;
            gameObject.SetActive(true);
            IsAgro = false;
            _tree.enabled = true;
        }

        public virtual void InActive()
        {
            IsDie = false;
            gameObject.SetActive(false);
            ResetHealth();
        }

        public void ApplyDamage(int damage)
        {
            _currentHealth = _enemyHealthModule.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, MaxHealth, damage);
            _agent.isStopped = false;

            if (_currentHealth <= 0)
            {
                if (_bossLevels.Contains((BossId)GetId()))
                    _finishPlate.OnActive();

                _tree.enabled = false;
                
                _currentHealth = 0;
                _healthBar.InActive();
                Died?.Invoke();
                IsDie = true;
                _agent.isStopped = true;
                EnemyAnimation.EnableDie();
            }
        }

        public void TakeDamage() =>
            _hero.ApplyDamage(Damage);

        public void Death()
        {
            SpawnLoot();
            InActive();
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