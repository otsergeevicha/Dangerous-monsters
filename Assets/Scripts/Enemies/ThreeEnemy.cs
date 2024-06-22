namespace Enemies
{
    public class ThreeEnemy : Enemy
    {
        protected override int GetId() =>
            (int)EnemyId.ThreeLevel;
    }
}