using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class IsFollowBase : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsFollowBase ? TaskStatus.Success : TaskStatus.Failure;
    }
}