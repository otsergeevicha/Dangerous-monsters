using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewTurretData", menuName = "Ammo/TurretData", order = 1)]
    public class TurretData : ScriptableObject
    {
        [Header("Detection radius")]
        [Range(5f, 25f)]
        public float RadiusDetection = 5f;

        [Range(145f, 195f)] 
        public float RotateSpeed = 195f;
    }
}