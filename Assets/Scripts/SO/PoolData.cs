using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewPool", menuName = "Pools/Create", order = 1)]
    public class PoolData : ScriptableObject
    {
        [Header("AmmoBox pool Player")]
        [Range(1, 15)] public int SizeAmmoBox = 5;
        
        [Header("AmmoBox pool Assistant")]
        [Range(1, 10)] public int SizeBasketAssistant = 5;
    }
}