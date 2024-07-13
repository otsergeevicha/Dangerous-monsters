using System;
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
        public EnemyAnimation EnemyAnimation;
        
        private readonly BossId[] _bossLevels = Enum.GetValues(typeof(BossId)).Cast<BossId>().ToArray();
        private readonly float _agroDistance = 8;
        private readonly float _attackDistance = 1.5f;

        protected int MaxHealth;
        protected int Damage;

        private HealthBar _healthBar;
        private LootSpawner _lootSpawner;
        private EnemyHealthModule _enemyHealthModule;
        private FinishPlate _finishPlate;

        private int _currentHealth;
        private Hero _hero;
        private Vector3 _baseGate;
        private bool _onMap;

        public event Action Died;
        public EnemyData EnemyData { get; private set; }
        public bool IsFollowBase { get; private set; }
        public bool IsAgro { get; private set; }
        public bool IsAttackRange { get; private set; }
        public bool IsDie { get; private set; }
        public Vector3 GetTarget { get; private set; }

        protected abstract int GetId();
        protected abstract void SetCurrentHealth();
        protected abstract void SetCurrentDamage();

        public void Construct(EnemyData enemyData,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, HealthBar healthBar, FinishPlate finishPlate,
            Hero hero, Vector3 baseGate)
        {
            _baseGate = baseGate;
            _hero = hero;
            _finishPlate = finishPlate;
            _healthBar = healthBar;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;
            EnemyData = enemyData;

            SetCurrentHealth();
            SetCurrentDamage();
            
            EnemyAnimation.Construct(enemyData);
            
            EnemyAnimation.IsDamaged += TakeDamage;
            EnemyAnimation.AttackEnded += EndAttack;
            EnemyAnimation.DiedComplete += Death;

            ResetHealth();
        }

        protected override void OnDisabled()
        {
            EnemyAnimation.IsDamaged -= TakeDamage;
            EnemyAnimation.AttackEnded -= EndAttack;
            EnemyAnimation.DiedComplete -= Death;
        }

        private void OnValidate() => 
            EnemyAnimation ??= ChildrenGet<EnemyAnimation>();

        protected override void UpdateCached()
        {
            if (!_onMap)
                return;

            float currentDistance = Vector3.Distance(transform.position, _hero.transform.position);

            if (currentDistance <= _attackDistance)
            {
                GetTarget = _hero.transform.position;
                IsAgro = false;
                IsFollowBase = false;
                IsAttackRange = true;
                return;
            }

            if (currentDistance <= _agroDistance)
            {
                GetTarget = _hero.transform.position;
                IsFollowBase = false;
                IsAttackRange = false;
                IsAgro = true;
                return;
            }
            
            GetTarget = new Vector3(_baseGate.x, 0, _baseGate.z);
            IsAgro = false;
            IsAttackRange = false;
            IsFollowBase = true;
        }

        private void TakeDamage() =>
            _hero.ApplyDamage(Damage);

        private void EndAttack() =>
            IsAttackRange = false;

        private void Death()
        {
            IsDie = false;
            Died?.Invoke();
            SpawnLoot();
            InActive();
        }

        public void ApplyDamage(int damage)
        {
            EnemyAnimation.EnableHit();
            
            _currentHealth = _enemyHealthModule.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, MaxHealth);

            if (_currentHealth <= 0)
            {
                if (_bossLevels.Contains((BossId)GetId()))
                    _finishPlate.OnActive();

                _currentHealth = 0;
                
                _onMap = false;
                IsFollowBase = false;
                IsAttackRange = false;
                IsAgro = false;
                IsDie = true;
            }
        }

        public virtual void OnActive()
        {
            _onMap = true;
            IsDie = false;
            gameObject.SetActive(true);
        }

        public virtual void InActive()
        {
            _onMap = false;
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