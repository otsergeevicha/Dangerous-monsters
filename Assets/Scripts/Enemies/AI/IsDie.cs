using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class IsDie : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsDie ? TaskStatus.Success : TaskStatus.Failure;
    }
}