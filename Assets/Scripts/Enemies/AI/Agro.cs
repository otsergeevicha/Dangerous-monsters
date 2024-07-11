using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class Agro : EnemyAction
    {
        public override void OnStart() => 
            Enemy.EnemyAnimation.EnableAgro();
    }
}