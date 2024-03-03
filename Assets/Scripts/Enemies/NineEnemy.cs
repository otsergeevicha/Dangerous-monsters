using System;
using SO;

namespace Enemies
{
    public class NineEnemy : Enemy
    {
        public EnemyData EnemyData { get; private set; }

        public override int GetId() => 
            (int)EnemyId.NineLevel;

        public override void Construct(EnemyData enemyData) => 
            EnemyData = enemyData;

        public override void OnActive() => 
            gameObject.SetActive(true);

        public override void InActive() => 
            gameObject.SetActive(false);

        public override event Action Died;
    }
}