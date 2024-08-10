using Enemies.AI.Parent;
using UnityEngine;

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
            if (InZone()) 
                Enemy.EnemyAnimation.EnableAttack();
            else
                Enemy.ResetBehaviorTree();
        }

        private bool InZone() =>
            Vector3.Distance(Enemy.CashTransform.position, Enemy.GetCurrentTarget) <=
            Enemy.EnemyData.AttackDistance;
    }
}