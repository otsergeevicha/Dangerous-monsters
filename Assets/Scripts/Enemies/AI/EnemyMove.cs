using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class EnemyMove : EnemyAction
    {
        public override void OnStart()
        {
            Enemy.EnemyAnimation.EnablePursuit();
            Agent.speed = Enemy.EnemyData.Speed;
            Agent.destination = Enemy.GetTarget;
        }
    }
}