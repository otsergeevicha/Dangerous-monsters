using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Characters/Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [HideInInspector] public int IdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int RunHash = Animator.StringToHash("Run");
        
        [HideInInspector] public float DeviationAmount = 15f;

        [Header("Health enemies")] 
        [Range(1, 50)]
        public int ZeroLevelHealth = 1;
        [Range(1, 50)]
        public int OneLevelHealth = 1;
        [Range(1, 50)]
        public int TwoLevelHealth = 1;
        [Range(1, 50)]
        public int ThreeLevelHealth = 1;
        [Range(1, 50)]
        public int FourLevelHealth = 1;
        [Range(1, 50)]
        public int FiveLevelHealth = 1;
        [Range(1, 50)]
        public int SixLevelHealth = 1;
        [Range(1, 50)]
        public int SevenLevelHealth = 1;
        [Range(1, 50)]
        public int EightLevelHealth = 1;
        [Range(1, 50)]
        public int NineLevelHealth = 1;
    }
}