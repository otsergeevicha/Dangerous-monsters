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

        public void InterstitialAd(Action action)
        {
            Agava.YandexGames.InterstitialAd.Show(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
        }

        private void OnOpenCallback()
        {
            throw new NotImplementedException();
        }

        private void OnCloseCallback(bool flag)
        {
            throw new NotImplementedException();
        }

        private void OnCloseCallback()
        {
            throw new NotImplementedException();
        }

        private void OnOfflineCallback()
        {
            throw new NotImplementedException();
        }

        private void OnErrorCallback(string obj)
        {
            throw new NotImplementedException();
        }
    }
}