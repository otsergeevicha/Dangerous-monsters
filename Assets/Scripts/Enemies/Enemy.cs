using Enemies.AI;
using Enemies.Animation;
using HpBar;
using Loots;
using Modules;
using Plugins.MonoCache;
using SO;
using Spawners;
using UnityEngine;
using UnityEngine.AI;
using Action = System.Action;

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
        public abstract int GetId();
        public abstract void Construct(EnemyData enemyData, DirectionOperator directionOperator,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, HealthBar healthBar);
        public abstract void OnActive();
        public abstract void InActive();
        public abstract Vector3 GetDirection();
        public abstract void SetReached(bool flag);
        public abstract bool IsCalm { get; protected set; }
        public abstract bool IsReached { get; protected set; }
        public abstract void ApplyDamage(int damage);
        public abstract event Action Died;

    }
}