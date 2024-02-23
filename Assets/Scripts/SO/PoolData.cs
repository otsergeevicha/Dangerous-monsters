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
    }
}