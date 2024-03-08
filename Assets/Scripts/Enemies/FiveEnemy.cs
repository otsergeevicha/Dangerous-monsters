using System;
using Enemies.AI;
using Reflex;
using SO;
using UnityEngine;

namespace Enemies
{
    public class FiveEnemy : Enemy
    {
        public EnemyData EnemyData { get; private set; }

        public override int GetId() => 
            (int)EnemyId.FiveLevel;

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

        public override bool IsCalm { get; protected set; }

        public override event Action Died;
    }
}