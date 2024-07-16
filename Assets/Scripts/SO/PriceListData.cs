using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewPriceList", menuName = "Price List", order = 1)]
    public class PriceListData : ScriptableObject
    {
        [Range(1, 1000)] 
        public int SectionPriceMultiplier = 100;

        [Range(3, 1000)] 
        public int PriceTransitionPlate = 3;

        [Range(3, 1000)] 
        public int MultiplierIncreasePrice = 3;

        [Header("Price upgrade hero")] 
        [Range(0, 1000)]
        public int PriceHeroHealth = 1;
        [Range(3, 1000)] 
        public int MultiplierPriceHeroHealth = 3;
        [Range(10, 100)] 
        public int StepIncreaseHealthHero = 10;
        
        [Range(0, 1000)]
        public int PriceHeroSpeed = 1;
        [Range(3, 1000)] 
        public int MultiplierPriceHeroSpeed = 3;
        
        [Range(0, 1000)]
        public int PriceHeroBasket = 1;
        [Range(3, 1000)] 
        public int MultiplierPriceHeroBasket = 3;
        
        [Range(0, 1000)]
        public int PriceHeroFiringRange = 1;
        [Range(3, 1000)] 
        public int MultiplierPriceHeroFiringRange = 3;
    }
}