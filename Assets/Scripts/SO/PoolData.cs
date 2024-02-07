using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewPool", menuName = "Pools/Create", order = 1)]
    public class PoolData : ScriptableObject
    {
        [Header("AmmoBox pool")]
        [Range(1, 15)] public int SizeAmmoBox = 5;
    }
}