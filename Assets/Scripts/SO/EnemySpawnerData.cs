using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewSpawner", menuName = "Spawners/EnemySpawner", order = 1)]
    public class EnemySpawnerData : ScriptableObject
    {
        [Range(0, 5000)] public int MillisecondsIntervalSpawn = 100;
    }
}