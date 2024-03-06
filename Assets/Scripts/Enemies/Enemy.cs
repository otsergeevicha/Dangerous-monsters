﻿using Enemies.Animation;
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
        public abstract Vector3 GetDirection();
        public abstract bool IsCalm { get; protected set; }
        public abstract event Action Died;
    }
}