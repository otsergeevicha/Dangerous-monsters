﻿using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace Enemies.AI.Parent
{
    public class EnemyConditional : Action
    {
        protected Enemy Enemy;
        protected NavMeshAgent Agent;

        public override void OnAwake()
        {
            Enemy ??= GetComponent<Enemy>();
            Agent ??= GetComponent<NavMeshAgent>();
        }
    }
}