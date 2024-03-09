using UnityEngine;

namespace Enemies.AI
{
    // public class DirectionOperator
    // {
    //     public Vector3 Generate(Vector3 ourPosition, Vector3 endPosition, float deviationAmount) => 
    //         ourPosition + GetNewVector(GetDirection(ourPosition, endPosition), GetRandomDeviation(deviationAmount));
    //     
    //     private Vector3 GetNewVector(Vector3 direction, float randomDeviation) => 
    //         Quaternion.Euler(0, 90, 0) * direction * randomDeviation;
    //     
    //     private float GetRandomDeviation(float deviationAmount) => 
    //         Random.Range(-1f, 1f) >= 0 ? deviationAmount : -deviationAmount;
    //     
    //     private Vector3 GetDirection(Vector3 ourPosition, Vector3 endPosition) => 
    //         (endPosition - ourPosition).normalized;
    // }
    
    public class DirectionOperator
    {
        public Vector3 Generate(Vector3 ourPosition, Vector3 endPosition, float deviationAmount, float weightTowardsEnd)
        {
            // Вычисляем направление движения от ourPosition к endPosition
            Vector3 direction = (endPosition - ourPosition).normalized;

            // Генерируем случайное число для определения направления
            float randomValue = Random.Range(0f, 1f);

            // Если случайное значение меньше или равно весу к endPoint, двигаемся к endPoint
            if (randomValue <= weightTowardsEnd)
            {
                return ourPosition + direction * deviationAmount;
            }
            else // Иначе двигаемся в случайном направлении
            {
                // Генерируем случайное отклонение влево или вправо
                float randomDeviation = Random.Range(-1f, 1f) >= 0 ? deviationAmount : -deviationAmount;

                // Вычисляем новое направление с учетом отклонения
                Vector3 deviationVector = Quaternion.Euler(0, 90, 0) * direction * randomDeviation;

                // Получаем точку между ourPosition и endPosition с учетом отклонения
                Vector3 newPosition = ourPosition + deviationVector;

                return newPosition;
            }
        }
    }
}