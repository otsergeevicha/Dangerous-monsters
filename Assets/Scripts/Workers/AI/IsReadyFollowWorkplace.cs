using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class IsReadyFollowWorkplace : WorkerConditional
    {
        public override TaskStatus OnUpdate() => 
            Worker.IsReadyWork ? TaskStatus.Success : TaskStatus.Failure;
    }
}