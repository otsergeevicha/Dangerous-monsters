using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class IsHandUnder : WorkerConditional
    {
        public override TaskStatus OnUpdate() => 
            Worker.IsHandEmpty ? TaskStatus.Success : TaskStatus.Failure;
    }
}