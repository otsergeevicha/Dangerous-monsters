namespace Canvases.UpgradePlayer
{
    public class HeroHealthLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroHealth;

        protected override void UpdateValue() => 
            CurrentValue = HeroData.MaxHealth;

        protected override void Upgrade() => 
            HeroData.MaxHealth += PriceList.StepIncreaseHealthHero;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroHealth += PriceList.MultiplierPriceHeroHealth;

        protected override bool CheckUpperLimit() => 
            HeroData.MaxHealth == HeroData.UpperLimitMaxHealth;
    }
}