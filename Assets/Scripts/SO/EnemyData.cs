using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Characters/Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [HideInInspector] public int IdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int RunHash = Animator.StringToHash("Run");
        
        [HideInInspector] public float DeviationAmount = 15f;

        [Header("Health simple enemies")] 
        [Range(1, 50)]
        public int ZeroEnemyHealth = 1;
        [Range(1, 50)]
        public int OneEnemyHealth = 1;
        [Range(1, 50)]
        public int TwoEnemyHealth = 1;
        [Range(1, 50)]
        public int ThreeEnemyHealth = 1;
        [Range(1, 50)]
        public int FourEnemyHealth = 1;
        [Range(1, 50)]
        public int FiveEnemyHealth = 1;
        [Range(1, 50)]
        public int SixEnemyHealth = 1;
        [Range(1, 50)]
        public int SevenEnemyHealth = 1;
        [Range(1, 50)]
        public int EightEnemyHealth = 1;
        [Range(1, 50)]
        public int NineEnemyHealth = 1;
        
        [Header("Health boss enemies")] 
        [Range(1, 500)]
        public int OneBossHealth = 1;
        [Range(1, 500)]
        public int TwoBossHealth = 1;
        [Range(1, 500)]
        public int ThreeBossHealth = 1;
        [Range(1, 500)]
        public int FourBossHealth = 1;
        [Range(1, 500)]
        public int FiveBossHealth = 1;
        [Range(1, 500)]
        public int SixBossHealth = 1;
        [Range(1, 500)]
        public int SevenBossHealth = 1;
        [Range(1, 500)]
        public int EightBossHealth = 1;
        [Range(1, 500)]
        public int NineBossHealth = 1;
        [Range(1, 500)]
        public int TenBossHealth = 1;
    }
}