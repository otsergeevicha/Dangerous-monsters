using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class IsBattle : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsBattle ? TaskStatus.Success : TaskStatus.Failure;
    }
}