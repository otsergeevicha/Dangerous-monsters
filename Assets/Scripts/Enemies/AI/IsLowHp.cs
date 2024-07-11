using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class IsLowHp : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsLowHp ? TaskStatus.Success : TaskStatus.Failure;
    }
}