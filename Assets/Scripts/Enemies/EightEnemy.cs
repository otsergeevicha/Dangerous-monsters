using System;
using Enemies.AI;
using SO;
using UnityEngine;

namespace Enemies
{
    public class EightEnemy : Enemy
    {
        public EnemyData EnemyData { get; private set; }

        public override int GetId() => 
            (int)EnemyId.EightLevel;

        public override void Construct(EnemyData enemyData, DirectionOperator directionOperator) => 
            EnemyData = enemyData;

        public override void OnActive() => 
            gameObject.SetActive(true);

        public override void InActive() => 
            gameObject.SetActive(false);

        public override Vector3 GetDirection()
        {
            throw new NotImplementedException();
        }

        public override void SetReached(bool flag) => 
            IsReached = flag;
        
        public override bool IsCalm { get; protected set; }
        public override bool IsReached { get; protected set; }

        public override event Action Died;
    }
}