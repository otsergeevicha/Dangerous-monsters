using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parents;

namespace Enemies
{
    public class IsCalmPatrol : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            EnemyAI.IsCalm ? TaskStatus.Success : TaskStatus.Failure;
    }
}