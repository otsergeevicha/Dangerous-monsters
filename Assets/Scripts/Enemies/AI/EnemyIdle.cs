using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class EnemyIdle : EnemyAction
    {
        public override void OnStart() => 
            Enemy.EnemyAnimation.EnableIdle();
    }
}