using SO;

namespace Enemies
{
    public class EightEnemy : Enemy
    {
        private EnemyData _enemyData;

        public override int GetId() => 
            (int)EnemyId.EightLevel;

        public override void Construct(EnemyData enemyData) => 
            _enemyData = enemyData;

        public override void OnActive() => 
            gameObject.SetActive(true);

        public override void InActive() => 
            gameObject.SetActive(false);
    }
}