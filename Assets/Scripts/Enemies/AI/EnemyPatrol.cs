using Enemies.AI.Parents;

namespace Enemies.AI
{
    public class EnemyPatrol : EnemyAction
    {
        public override void OnStart() => 
            Agent.SetDestination(Enemy.GetDirection());
    }
}