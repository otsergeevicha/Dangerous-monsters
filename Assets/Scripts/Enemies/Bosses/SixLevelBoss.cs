namespace Enemies.Bosses
{
    public class SixLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.SixLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.SixBossHealth;
    }
}