using BehaviorDesigner.Runtime.Tasks;
using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class IsFear : EnemyConditional
    {
        public override TaskStatus OnUpdate() => 
            Enemy.IsFear ? TaskStatus.Success : TaskStatus.Failure;
    }
}