using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parents;

namespace Enemies.AI
{
    public class IsReachedTarget : EnemyConditional
    {
        public override TaskStatus OnUpdate() =>
            Agent.remainingDistance >= Agent.stoppingDistance ? TaskStatus.Failure : TaskStatus.Success;
    }
}