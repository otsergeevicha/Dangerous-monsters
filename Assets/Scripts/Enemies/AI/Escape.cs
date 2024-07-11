using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class Escape : EnemyAction
    {
        public override void OnStart()
        {
            Enemy.EnemyAnimation.EnablePursuit();
            Agent.speed = Enemy.EnemyData.EscapeSpeed;
            Agent.SetDestination(Enemy.GetTarget);
        }
    }
}