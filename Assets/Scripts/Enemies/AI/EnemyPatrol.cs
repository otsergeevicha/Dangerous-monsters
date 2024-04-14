using Enemies.AI.Parents;

namespace Enemies.AI
{
    public class EnemyPatrol : EnemyAction
    {
         public override void OnStart()
         {
             if (Enemy.IsReached) 
                 Agent.SetDestination(Enemy.GetDirection());
         }
    }
}