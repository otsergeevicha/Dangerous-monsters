using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class EnemyAttack : EnemyAction
    {
        public override void OnStart()
        {
            Attack();
            Enemy.EnemyAnimation.OnAttacked += Attack;
        }

        public override void OnEnd() =>
            Enemy.EnemyAnimation.OnAttacked -= Attack;

        private void Attack()
        {
            if (Enemy.InZone()) 
                Enemy.EnemyAnimation.EnableAttack();
            else
                Enemy.ResetBehaviorTree();
        }
    }
}