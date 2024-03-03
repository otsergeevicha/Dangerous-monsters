using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parents;
using Enemies.Animation;
using Plugins.MonoCache;
using SO;
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
        public abstract void Construct(EnemyData enemyData);
        public abstract void OnActive();
        public abstract void InActive();
        public abstract event Action Died;
    }

    // public class IsCalmPatrol : EnemyConditional
    // {
    //     public override TaskStatus OnUpdate() => 
    //         Enemy.IsCalm ? TaskStatus.Success : TaskStatus.Failure;
    // }
    //
    // public class EnemyPatrol : EnemyAction
    // {
    //     public override void OnStart() => 
    //         Agent.SetDestination(Enemy.GetDirection());
    // }
    //
    // public class IsReachedTarget : EnemyConditional
    // {
    //     public override TaskStatus OnUpdate() =>
    //         Agent.remainingDistance <= Agent.stoppingDistance ? TaskStatus.Failure : TaskStatus.Success;
    // }
}