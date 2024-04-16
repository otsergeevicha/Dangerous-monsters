using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Services.SDK;

namespace Infrastructure.SDK
{
    public class SDKService : ISDKService
    {
        public async UniTaskVoid OnActive(Action completed, Action<string> error = null)
        {
#if UNITY_EDITOR
            completed?.Invoke();
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
    }
}