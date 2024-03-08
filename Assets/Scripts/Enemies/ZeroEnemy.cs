using System;
using Enemies.AI;
using SO;
using UnityEngine;

namespace Enemies
{
    public class ZeroEnemy : Enemy
    {
        private DirectionOperator _directionOperator;
        public EnemyData EnemyData { get; private set; }

        public override int GetId() => 
            (int)EnemyId.ZeroLevel;

        public override void Construct(EnemyData enemyData, DirectionOperator directionOperator)
        {
            _directionOperator = directionOperator;
            EnemyData = enemyData;
        }

        public override void OnActive() => 
            gameObject.SetActive(true);

        public override void InActive() => 
            gameObject.SetActive(false);

        public override Vector3 GetDirection() => 
            _directionOperator.Generate(transform.position, Vector3.zero, EnemyData.DeviationAmount);

        public override bool IsCalm { get; protected set; } = true;

        public override event Action Died;
    }
}