namespace Enemies
{
    public class OneEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.OneLevel;
    }
}