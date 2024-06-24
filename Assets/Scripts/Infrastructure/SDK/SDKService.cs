using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Services.SDK;
using UnityEngine;

namespace Infrastructure.SDK
{
    public class SDKService : ISDKService
    {
        private YandexSdk _yandex = new ();
        
        public async UniTaskVoid OnActive(Action completed, Action<string> error = null)
        {
#if UNITY_EDITOR
            completed?.Invoke();
            Debug.Log("SDK инициализировано");
            return;
#endif
            
            try
            {
                await YandexGamesSdk.Initialize(() => 
                    completed?.Invoke());
            }
            catch (Exception description)
            {
                error?.Invoke(description.Message);
            }
        }

        public void AdReward(Action rewardCompleted)
        {
#if UNITY_EDITOR
            rewardCompleted?.Invoke();
            Debug.Log("Реклама показана");
            return;
#endif
            
            _yandex.ShowReward(() => rewardCompleted?.Invoke());
        }
    }
}