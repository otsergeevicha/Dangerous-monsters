using System;
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
    [RequireComponent(typeof(EnemyTriggers))]
    public abstract class Enemy : MonoCache
    {
        [HideInInspector] [SerializeField] private EnemyTriggers _enemyTriggers;

        [HideInInspector] public EnemyAnimation EnemyAnimation;

        private readonly BossId[] _bossLevels = Enum.GetValues(typeof(BossId)).Cast<BossId>().ToArray();

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
        private float _agroDistance;
        private float _timePursuit;
        private float _attackRange;

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
            EnemyAnimation.DiedComplete += Death;

            ResetHealth();

            _agroDistance = EnemyData.AgroDistance;
            _attackRange = EnemyData.AttackDistance;
            _timePursuit = EnemyData.TimePursuit;

            _enemyTriggers.Construct(this);
            _enemyTriggers.SetAgroZone(_agroDistance);
        }

        protected override void OnDisabled() =>
            DisposeDependency();

        private void OnValidate()
        {
            EnemyAnimation ??= ChildrenGet<EnemyAnimation>();
            _enemyTriggers ??= Get<EnemyTriggers>();
        }

        protected override void UpdateCached()
        {
            if (IsFollowBase)
            {
                GetTarget = _baseGate;
                return;
            }

            float currentDistance = Vector3.Distance(transform.position, _hero.transform.position);

            IsAttackRange = currentDistance <= _attackRange;

            if (currentDistance >= _agroDistance)
            {
                GetTarget = _hero.transform.position;

                _timePursuit -= Time.deltaTime;

                if (Mathf.Approximately(_timePursuit, Single.Epsilon))
                {
                    _timePursuit = EnemyData.TimePursuit;
                    IsFollowBase = true;
                }
            }
            else
                _timePursuit = EnemyData.TimePursuit;
        }

        public void OnAgro()
        {
            IsFollowBase = false;
            IsAgro = true;
        }

        private void TakeDamage() =>
            _hero.ApplyDamage(Damage);

        private void Death()
        {
            Died?.Invoke();
            SpawnLoot();
            InActive();
            IsDie = false;
        }

        public void ApplyDamage(int damage)
        {
            EnemyAnimation.EnableHit();

            _currentHealth = _enemyHealthModule.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, MaxHealth, damage);

            if (_currentHealth <= 0)
            {
                if (_bossLevels.Contains((BossId)GetId()))
                    _finishPlate.OnActive();

                _currentHealth = 0;
                IsDie = true;
            }
        }

        public virtual void OnActive()
        {
            IsFollowBase = true;
            gameObject.SetActive(true);
        }

        public virtual void InActive()
        {
            gameObject.SetActive(false);
            ResetHealth();
        }

        public void OnDestroy()
        {
            DisposeDependency();

            Destroy(_healthBar);
            Destroy(this);
        }

        private void SpawnLoot() =>
            _lootSpawner.SpawnMoney(GetId(), transform.position);

        private void ResetHealth() =>
            _currentHealth = MaxHealth;

        private void DisposeDependency()
        {
            EnemyAnimation.IsDamaged -= TakeDamage;
            EnemyAnimation.DiedComplete -= Death;
        }
    }
}