using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewPriceList", menuName = "Price List", order = 1)]
    public class PriceListData : ScriptableObject
    {
        [Range(1, 1000)]
        public int SectionPriceMultiplier = 100;
    }
}