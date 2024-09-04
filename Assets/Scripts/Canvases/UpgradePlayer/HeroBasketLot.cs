using GameAnalyticsSDK;

namespace Canvases.UpgradePlayer
{
    public class HeroBasketLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroBasket;

        protected override void UpdateValue()
        {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"UpgradePlayer:SizeBasket:{HeroData.SizeBasket}");
#endif
            CurrentValue = HeroData.SizeBasket;
        }

        protected override void Upgrade() =>
            HeroData.SizeBasket++;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroBasket += PriceList.MultiplierPriceHeroBasket;
        
        protected override bool CheckUpperLimit() => 
            HeroData.SizeBasket == HeroData.UpperLimitBasket;
    }
}