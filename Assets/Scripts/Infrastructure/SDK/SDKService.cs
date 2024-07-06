using System;
using Services.SDK;
using UnityEngine;

namespace Infrastructure.SDK
{
    public class SDKService : ISDKService
    {
        private readonly YandexSdk _yandex = new();

        public void AdReward(Action rewardCompleted)
        {
#if UNITY_EDITOR
            rewardCompleted?.Invoke();
            Debug.Log("AD showed");
            return;
#endif

            _yandex.ShowReward(() =>
                rewardCompleted?.Invoke());
        }
    }
}