using System;
using Enemies.AI;
using SO;
using UnityEngine;

namespace Enemies
{
    public class TwoEnemy : Enemy
    {
        private DirectionOperator _directionOperator;
        public override event Action Died;
        public EnemyData EnemyData { get; private set; }
        public override bool IsCalm { get; protected set; } = true;
        public override bool IsReached { get; protected set; }

        public override int GetId() => 
            (int)EnemyId.TwoLevel;

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

        public override void SetReached(bool flag) => 
            IsReached = flag;
    }
}