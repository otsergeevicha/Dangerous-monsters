using System;
using Agava.YandexGames;

namespace Infrastructure.SDK
{
    public class YandexSdk
    {
        public void ShowReward(Action onCompleted)
        {
            VideoAd.Show(OnOpenCallback, () => onCompleted?.Invoke(), OnCloseCallback, OnErrorCallback);
        }

        private void OnOpenCallback()
        {
            throw new NotImplementedException();
        }

        private void OnRewardedCallback()
        {
            throw new NotImplementedException();
        }

        private void OnCloseCallback()
        {
            throw new NotImplementedException();
        }

        private void OnErrorCallback(string obj)
        {
            throw new NotImplementedException();
        }
    }
}