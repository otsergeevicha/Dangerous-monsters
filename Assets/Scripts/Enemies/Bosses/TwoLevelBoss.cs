namespace Enemies.Bosses
{
    public class TwoLevelBoss : Enemy
    {
        protected override int GetId() =>
            (int)BossId.TwoLevel;
        
        protected override void SetCurrentHealth() => 
            MaxHealth = EnemyData.TwoBossHealth;
    }
}