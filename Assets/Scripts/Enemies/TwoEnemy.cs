namespace Enemies
{
    public class TwoEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.TwoLevel;
    }
}