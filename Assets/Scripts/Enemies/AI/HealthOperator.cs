namespace Enemies.AI
{
    public class HealthOperator
    {
        //the class was created with a foundation for the development and creation of a more complex logic for calculating health
        public int CalculateDamage(int currentHealth, int damage) => 
            currentHealth - damage;
    }
}