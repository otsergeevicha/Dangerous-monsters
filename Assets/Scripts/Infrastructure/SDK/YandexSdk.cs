using System;
using Agava.YandexGames;
using UnityEngine;

namespace Infrastructure.SDK
{
    public class YandexSdk
    {
        private AudioListener _audioListener;

        public void Inject(AudioListener audioListener) =>
            _audioListener = audioListener;

        public void ShowReward(Action onCompleted)
        {
            VideoAd.Show(Mute
                , () =>
                {
                    onCompleted?.Invoke();
                    UnMute();
                }, UnMute
                , (string _) => { UnMute(); });
        }

        public void InterstitialAd(Action action)
        {
            Agava.YandexGames.InterstitialAd.Show(Mute
                , (bool _) => action?.Invoke()
                , (string _) =>
                {
                    action?.Invoke();
                    UnMute();
                }, () =>
                {
                    action?.Invoke();
                    UnMute();
                });
        }

        private void Mute()
        {
            Time.timeScale = 0;
            _audioListener.enabled = false;
        }

        private void UnMute()
        {
            Time.timeScale = 1;
            _audioListener.enabled = true;
        }
    }
}