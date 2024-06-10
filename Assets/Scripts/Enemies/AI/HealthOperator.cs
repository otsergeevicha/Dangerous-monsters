namespace Enemies.AI
{
    public class HealthOperator
    {
        public int CalculateDamage(int currentHealth, int damage) => 
            currentHealth - damage;
    }
}