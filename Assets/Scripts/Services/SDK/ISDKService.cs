using System;

namespace Services.SDK
{
    public interface ISDKService
    {
        void AdReward(Action rewardCompleted);
        void ShowInterstitial(Action adCompleted);
    }
}