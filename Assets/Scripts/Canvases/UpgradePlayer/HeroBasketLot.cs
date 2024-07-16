namespace Canvases.UpgradePlayer
{
    public class HeroBasketLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroBasket;

        protected override void UpdateValue() => 
            CurrentValue = HeroData.SizeBasket;
        
        protected override void Upgrade() =>
            HeroData.SizeBasket++;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroBasket += PriceList.MultiplierPriceHeroBasket;
        
        protected override bool CheckUpperLimit() => 
            HeroData.SizeBasket == HeroData.UpperLimitBasket;
    }
}