namespace Canvases.UpgradePlayer
{
    public class HeroSpeedLot : HeroLot
    {
        protected override void UpdatePrice() => 
            CurrentPrice = PriceList.PriceHeroSpeed;

        protected override void UpdateValue() => 
            CurrentValue = HeroData.Speed;

        protected override void Upgrade() =>
            HeroData.Speed++;

        protected override void IncreasePrice() => 
            PriceList.PriceHeroSpeed += PriceList.MultiplierPriceHeroSpeed;
        
        protected override bool CheckUpperLimit() => 
            HeroData.Speed == HeroData.UpperLimitSpeed;
    }
}