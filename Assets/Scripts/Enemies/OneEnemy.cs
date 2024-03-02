using SO;

namespace Enemies
{
    public class OneEnemy : Enemy
    {
        private EnemyData _enemyData;

        public override int GetId() => 
            (int)EnemyId.OneLevel;

        public override void Construct(EnemyData enemyData) => 
            _enemyData = enemyData;

        public override void OnActive() => 
            gameObject.SetActive(true);

        public override void InActive() => 
            gameObject.SetActive(false);
    }
}