namespace Enemies.SimpleEnemies
{
    public class ThreeEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.ThreeLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.ThreeEnemyHealth;
    }
}