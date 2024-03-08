using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Characters/Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [HideInInspector] public int IdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int RunHash = Animator.StringToHash("Run");
        
        [Header("Deviation direction")] [Range(0, 15)]
        public float DeviationAmount = 1f;
    }
}