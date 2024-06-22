namespace Enemies
{
    public class ZeroEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.ZeroLevel;
    }
}