using System;

namespace Infrastructure.GroupData
{
    [Serializable]
    public class DataStateGame
    {
        public bool FirstLaunch;
        
        public int CurrentLevel;
        
        public int HeroHealth;
        public int HeroSpeed;
        public int HeroSizeBasket;
        public float HeroRadiusDetection;
        
        public float RadiusDetection = 5f;
        public float RotateSpeed = 195f;
        
        public int SectionPriceMultiplier;
        public int PriceTransitionPlate = 3;
        public int MultiplierIncreasePrice = 3;
        public int StartPriceTurret = 100;    
        public int StepIncreasePriceTurret = 100;
        public int PriceHeroHealth = 50;
        public int MultiplierPriceHeroHealth = 3;
        public int StepIncreaseHealthHero = 10;
        public int PriceHeroSpeed = 50;
        public int PriceHeroBasket = 50;
        public int PriceHeroFiringRange = 50;
        public int LoseBonusMoney = 500;
        public int LoseBonusGem = 50;
    }
}