namespace Enemies.Bosses
{
    public class NineLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.NineLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.NineBossHealth;
    }
}