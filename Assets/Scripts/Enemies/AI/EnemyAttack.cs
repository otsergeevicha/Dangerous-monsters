using BehaviorDesigner.Runtime.Tasks;
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

        public override TaskStatus OnUpdate() =>
            Vector3.Distance(Enemy.CashTransform.position, Enemy.GetCurrentTarget) <= Enemy.EnemyData.AttackDistance
                ? TaskStatus.Running
                : TaskStatus.Failure;

        public override void OnEnd() => 
            Enemy.EnemyAnimation.OnAttacked -= Attack;

        private void Attack()
        {
            Enemy.CashTransform.forward = Enemy.GetCurrentTarget;
            Enemy.EnemyAnimation.EnableAttack();
        }
    }
}