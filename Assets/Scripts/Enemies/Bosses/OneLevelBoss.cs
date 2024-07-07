namespace Enemies.Bosses
{
    public class OneLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.OneLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.OneBossHealth;
    }
}