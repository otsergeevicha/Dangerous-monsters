using Enemies.AI.Parents;

namespace Enemies.AI
{
    public class IsReachedTarget : EnemyConditional
    {
        public override void OnStart() => 
            EnemyAI.SetReached(Agent.remainingDistance <= Agent.stoppingDistance);
    }
}