using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewPool", menuName = "Pools/Create", order = 1)]
    public class PoolData : ScriptableObject
    {
        [Range(4, 10)]
        public int MaxCountWorkers = 4;
            
        [Range(1, 10)]
        public int MaxCountLootBoxes = 5;
        [Range(0, 100)]
        public int PercentSpawnFreeLoot = 50;
        
        public readonly int SizeAmmoBoxPlayer = 15;

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
        
        [Header("Size pool missiles turret")]
        [Range(15, 30)] 
        public int MaxCountMissiles = 20;

        [Range(10, 30)] 
        public int MaxCountBullets = 10;

        [Range(30, 100)]
        public int MaxCountMoney = 30;
    }
}