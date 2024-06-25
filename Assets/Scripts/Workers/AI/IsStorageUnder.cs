using BehaviorDesigner.Runtime.Tasks;
using Workers.AI.Parents;

namespace Workers.AI
{
    public class IsStorageUnder : WorkerConditional
    {
        public override TaskStatus OnUpdate() =>
            Worker.IsStorageEmpty ? TaskStatus.Success : TaskStatus.Failure;
    }
}