using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class IsAttackRange : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsAttackRange ? TaskStatus.Success : TaskStatus.Failure;
    }
}