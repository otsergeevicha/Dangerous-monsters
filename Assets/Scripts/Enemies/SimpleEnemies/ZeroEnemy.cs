namespace Enemies.SimpleEnemies
{
    public class ZeroEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.ZeroLevel;

        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.ZeroEnemyHealth;
    }
}