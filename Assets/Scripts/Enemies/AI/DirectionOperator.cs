using UnityEngine;

namespace Enemies.AI
{
    public class DirectionOperator
    {
        public Vector3 Generate(Vector3 ourPosition, Vector3 endPosition, float deviationAmount)
        {
            Vector3 direction = (endPosition - ourPosition).normalized;

            if (Random.Range(0f, 1f) <= .5f)
                return ourPosition + direction * deviationAmount;
            else
                return ourPosition + GetDeviationVector(deviationAmount, direction);
        }

        private Vector3 GetDeviationVector(float deviationAmount, Vector3 direction) => 
            Quaternion.Euler(0, 90, 0) * direction * GetRandomDeviation(deviationAmount);

        private float GetRandomDeviation(float deviationAmount) => 
            Random.Range(-1f, 1f) >= 0 ? deviationAmount : -deviationAmount;
    }
}