using BehaviorDesigner.Runtime.Tasks;
using Enemies.Animation;
using UnityEngine.AI;

namespace Enemies.AI.Parents
{
    public class EnemyAction : Action
    {
        protected Enemy Enemy;
        protected NavMeshAgent Agent;
        protected EnemyAnimation EnemyAnimation;
        
        public override void OnAwake()
        {
            Enemy ??= GetComponent<Enemy>();
            Agent ??= GetComponent<NavMeshAgent>();
            EnemyAnimation ??= GetComponent<EnemyAnimation>();
        }
    }
}