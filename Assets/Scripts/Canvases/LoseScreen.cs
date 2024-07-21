using System;
using Plugins.MonoCache;
using Services.Bank;
using Services.SDK;
using SO;
using TMPro;
using UnityEngine;

namespace Canvases
{
    public class LoseScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;
        [SerializeField] private ParticleSystem _particle;

        [SerializeField] private TMP_Text _remainingMoney;
        [SerializeField] private TMP_Text _remainingGem;
        
        [SerializeField] private TMP_Text _bonusMoney;
        [SerializeField] private TMP_Text _bonusGem;
        
        public event Action OnClickReStart;
        
        private IWallet _wallet;
        private ISDKService _sdk;
        private bool _isAdShowed;
        
        private PriceListData _priceList;

        public void Construct(IWallet wallet, ISDKService sdk, PriceListData priceList)
        {
            _priceList = priceList;
            _sdk = sdk;
            _wallet = wallet;
        }
        
        private void Start()
        {
            _canvas.enabled = false;
            _particle.gameObject.SetActive(false);
        }

        private void OnValidate() => 
            _canvas ??= Get<Canvas>();

        
        public void RewardBonus()
        {
            _sdk.AdReward(delegate
            {
                _wallet.ApplyMoney(_priceList.LoseBonusMoney);
                _wallet.ApplyGem(_priceList.LoseBonusGem);

                _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
                _remainingGem.text = _wallet.ReadCurrentGem().ToString();

                _isAdShowed = true;
            });
        }
        public void OnActive()
        {
            Time.timeScale = 0;
            _particle.gameObject.SetActive(true);
            _canvas.enabled = true;

            _bonusMoney.text = _priceList.LoseBonusMoney.ToString();
            _bonusGem.text = _priceList.LoseBonusGem.ToString();
            
            _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
            _remainingGem.text = _wallet.ReadCurrentGem().ToString();
        }

        public void InActive()
        {
            if (_isAdShowed == false)
                _sdk.ShowInterstitial(TryAgain);
            else
                TryAgain();

            _isAdShowed = false;
        }

        private void TryAgain()
        {
            OnClickReStart?.Invoke();
            Time.timeScale = 1;
            _particle.gameObject.SetActive(false);
        }
    }
}