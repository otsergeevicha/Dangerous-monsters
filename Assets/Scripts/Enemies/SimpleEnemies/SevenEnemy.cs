namespace Enemies.SimpleEnemies
{
    public class SevenEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.SevenLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.SevenEnemyHealth;
    }
}