using System;
using System.Collections;
using System.Linq;
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
        [HideInInspector] [SerializeField] private EnemyTriggers _enemyTriggers;
        [HideInInspector] public EnemyAnimation EnemyAnimation;
        [SerializeField] private NavMeshAgent _agent;

        private readonly BossId[] _bossLevels = Enum.GetValues(typeof(BossId)).Cast<BossId>().ToArray();

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
        private bool _isAgro;
        private Vector3 _baseGate;
        
        public bool IsDie { get; private set; }
        protected EnemyData EnemyData { get; private set; }

        protected abstract int GetId();
        protected abstract void SetCurrentHealth();
        protected abstract void SetCurrentDamage();
        public event Action Died;

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

            EnemyAnimation.Construct(enemyData, this, _agent);

            _enemyTriggers.OnAgro += () =>
            {
                EnemyAnimation.EnableRun();

                _agent.destination = _hero.transform.position;
                _isAgro = true;
                _coroutine = StartCoroutine(CheckAttackRange());
            };

            _enemyTriggers.NonAgro += () =>
            {
                EnemyAnimation.EnableRun();

                _isAgro = false;
                _agent.destination = baseGate;

                if (_coroutine != null)
                    StopCoroutine(CheckAttackRange());
            };

            ResetHealth();

            _agent.speed = enemyData.Speed;
        }

        public void Escape()
        {
            _healthBar.InActive();
            Died?.Invoke();
            IsDie = true;
            _agent.isStopped = true;
            InActive();
        }

        protected override void UpdateCached()
        {
            if ((Vector3.Distance(transform.position, _hero.transform.position) > EnemyData.AgroDistance) && _agent.isStopped) 
                return;
            
            if (_isAgro)
            {
                EnemyAnimation.EnableRun();
                _agent.destination = _hero.transform.position;

                _coroutine = StartCoroutine(CheckAttackRange());
            }

            if (!_isAgro)
            {
                EnemyAnimation.EnableRun();
                _agent.destination = _baseGate;

                if (_coroutine != null)
                    StopCoroutine(CheckAttackRange());
            }
        }

        private void OnValidate()
        {
            EnemyAnimation ??= ChildrenGet<EnemyAnimation>();
            _enemyTriggers ??= ChildrenGet<EnemyTriggers>();
            _agent ??= Get<NavMeshAgent>();
        }

        public virtual void OnActive()
        {
            IsDie = false;
            gameObject.SetActive(true);

            EnemyAnimation.EnableRun();
            _isAgro = false;
            _agent.destination = _baseGate;

            if (_coroutine != null)
                StopCoroutine(CheckAttackRange());
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

                _currentHealth = 0;
                _healthBar.InActive();
                Died?.Invoke();
                IsDie = true;
                _agent.isStopped = true;
                EnemyAnimation.EnableDie();
            }
        }

        public void AttackCompleted()
        {
            if (Vector3.Distance(transform.position, _hero.transform.position) <= EnemyData.AttackDistance)
            {
                EnemyAnimation.EnableAttack();
                return;
            }

            if (_isAgro)
            {
                EnemyAnimation.EnableRun();
                _agent.destination = _hero.transform.position;

                _coroutine = StartCoroutine(CheckAttackRange());
            }

            if (!_isAgro)
            {
                EnemyAnimation.EnableRun();
                _agent.destination = _baseGate;

                if (_coroutine != null)
                    StopCoroutine(CheckAttackRange());
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

        private IEnumerator CheckAttackRange()
        {
            while (_isAgro)
            {
                if (Vector3.Distance(transform.position, _hero.transform.position) <= EnemyData.AttackDistance)
                {
                    EnemyAnimation.EnableAttack();
                    yield break;
                }

                yield return null;
            }
        }
    }
}