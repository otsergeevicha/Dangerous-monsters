using GameAnalyticsSDK;

namespace Canvases.UpgradePlayer
{
    public class HeroSpeedLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroSpeed;

        protected override void UpdateValue()
        {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"UpgradePlayer:Speed:{HeroData.Speed}");
#endif
            CurrentValue = HeroData.Speed;
        }

        protected override void Upgrade() =>
            HeroData.Speed++;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroSpeed += PriceList.MultiplierPriceHeroSpeed;
        
        protected override bool CheckUpperLimit() => 
            HeroData.Speed == HeroData.UpperLimitSpeed;
    }
}