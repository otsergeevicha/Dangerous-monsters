using UnityEngine;

namespace Enemies.AI
{
    public class DirectionOperator
    {
        public Vector3 Generate(Vector3 ourPosition, Vector3 endPosition, float deviationAmount) => 
            ourPosition + GetNewVector(GetDirection(ourPosition, endPosition), GetRandomDeviation(deviationAmount));

        private Vector3 GetNewVector(Vector3 direction, float randomDeviation) => 
            Quaternion.Euler(0, 90, 0) * direction * randomDeviation;

        private float GetRandomDeviation(float deviationAmount) => 
            Random.Range(-1f, 1f) >= 0 ? deviationAmount : -deviationAmount;

        private Vector3 GetDirection(Vector3 ourPosition, Vector3 endPosition) => 
            (endPosition - ourPosition).normalized;
    }
}