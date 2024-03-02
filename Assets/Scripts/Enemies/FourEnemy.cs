using SO;

namespace Enemies
{
    public class FourEnemy : Enemy
    {
        private EnemyData _enemyData;

        public override int GetId() => 
            (int)EnemyId.FourLevel;

        public override void Construct(EnemyData enemyData) => 
            _enemyData = enemyData;

        public override void OnActive() => 
            gameObject.SetActive(true);

        public override void InActive() => 
            gameObject.SetActive(false);
    }
}