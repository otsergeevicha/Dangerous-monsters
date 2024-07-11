using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class Enrage : EnemyAction
    {
        public override void OnStart() => 
            Enemy.EnemyAnimation.EnableEnrage();
    }
}