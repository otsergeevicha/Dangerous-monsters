using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class Die : EnemyAction
    {
        public override void OnStart() => 
            Enemy.EnemyAnimation.EnableDie();
    }
}