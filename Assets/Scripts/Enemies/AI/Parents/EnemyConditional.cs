using BehaviorDesigner.Runtime.Tasks;
using Enemies.Animation;
using UnityEngine.AI;

namespace Enemies.AI.Parents
{
    public class EnemyConditional : Conditional
    {
        protected Enemy EnemyAI;
        protected NavMeshAgent Agent;
        protected EnemyAnimation EnemyAnimation;
        
        public override void OnAwake()
        {
            EnemyAI = GetComponent<Enemy>();
            Agent = GetComponent<NavMeshAgent>();
            EnemyAnimation = GetComponent<EnemyAnimation>();
        }
    }
}