using GameAnalyticsSDK;
using UnityEngine;

namespace Canvases.UpgradePlayer
{
    public class HeroFiringRangeLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroFiringRange;

        protected override void UpdateValue()
        {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"UpgradePlayer:RadiusDetection:{HeroData.RadiusDetection}");
#endif
            CurrentValue = (int)HeroData.RadiusDetection;
        }

        protected override void Upgrade() =>
            HeroData.RadiusDetection++;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroFiringRange += PriceList.MultiplierPriceHeroFiringRange;
        
        protected override bool CheckUpperLimit() => 
            Mathf.Approximately(HeroData.RadiusDetection, HeroData.UpperLimitRadiusDetection);
    }
}