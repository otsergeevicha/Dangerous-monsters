using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class Attack : EnemyAction
    {
        public override void OnStart() => 
            Enemy.EnemyAnimation.EnableAttack();
    }
}