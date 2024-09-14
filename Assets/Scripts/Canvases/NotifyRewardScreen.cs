using System;
using GameAnalyticsSDK;
using Plugins.MonoCache;
using Services.SDK;
using TMPro;
using UnityEngine;

namespace Canvases
{
    public class NotifyRewardScreen : MonoCache
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _description;
        
        private ISDKService _sdk;
        private string _currentReward;
        private string _descriptionHolder;

        public event Action RewardCompleted;

        private void Start() => 
            _descriptionHolder = _description.text;

        public void Construct(ISDKService sdk) => 
            _sdk = sdk;

        public void OnActive(string currentReward)
        {
            _currentReward = currentReward;
            
            Time.timeScale = 0;
            _canvas.enabled = true;
            
            _description.text = $"{_descriptionHolder } {currentReward}";
        }

        public void InActive()
        {
            Time.timeScale = 1;
            _canvas.enabled = false;
        }

        public void WatchAds()
        {
#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Reward:{_currentReward}");
#endif
            _sdk.AdReward(() =>
            {
                RewardCompleted?.Invoke();
                InActive();
            });
        }
    }
}