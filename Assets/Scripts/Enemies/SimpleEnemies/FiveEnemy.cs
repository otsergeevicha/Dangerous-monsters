namespace Enemies.SimpleEnemies
{
    public class FiveEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.FiveLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.FiveEnemyHealth;
    }
}