﻿using Enemies.AI.Parent;

namespace Enemies.AI
{
    public class PursuitGoal : EnemyAction
    {
        public override void OnStart()
        {
            Enemy.EnemyAnimation.EnablePursuit();
            Agent.speed = Enemy.EnemyData.Speed;
            Agent.SetDestination(Enemy.GetTarget);
        }
    }
}