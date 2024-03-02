using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewPool", menuName = "Pools/Create", order = 1)]
    public class PoolData : ScriptableObject
    {
        [Range(1, 15)]
        public int SizeAmmoBoxPlayer = 5;

        [Range(1, 10)]
        public int SizeBasketAssistant = 5;

        [Range(1, 4)]
        public int MaxCountCargoAssistant = 1;

        [Range(1, 10)] 
        public int CurrentLevelGame = 1;

        [Header("Amount enemies")]
        [Range(1, 10)] 
        public int OneLevelCountEnemy = 9;
        [Range(1, 10)] 
        public int TwoLevelCountEnemy = 7;
        [Range(1, 10)] 
        public int ThreeLevelCountEnemy = 6;
        [Range(1, 10)] 
        public int FourLevelCountEnemy = 6;
        [Range(1, 10)] 
        public int FiveLevelCountEnemy = 6;
        [Range(1, 10)] 
        public int SixLevelCountEnemy = 6;
        [Range(1, 10)] 
        public int SevenLevelCountEnemy = 6;
        [Range(1, 10)] 
        public int EightLevelCountEnemy = 6;
        [Range(1, 10)] 
        public int NineLevelCountEnemy = 7;
        [Range(1, 10)] 
        public int TenLevelCountEnemy = 9;
    }
}