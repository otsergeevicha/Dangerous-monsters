using Player;
using Plugins.MonoCache;
using Services.Bank;
using Services.SDK;
using SO;
using TMPro;
using UnityEngine;

namespace Canvases.UpgradePlayer
{
    public abstract class HeroLot : MonoCache
    {
        [SerializeField] protected TMP_Text ValueView;
        
        [SerializeField] protected TMP_Text PriceView;
        
        [SerializeField] protected GameObject Ad;
        
        [SerializeField] protected GameObject MaxLevel;
        [SerializeField] protected GameObject ReadyUpgrade;

        private const string FreeLot = "Free";
        
        protected int CurrentValue;
        protected int CurrentPrice;
        
        protected HeroData HeroData;
        protected PriceListData PriceList;
        
        private UpgradeHeroScreen _upgradeHeroScreen;
        
        private IWallet _wallet;
        private int _tempPrice;
        private Hero _hero;
        private ISDKService _sdk;

        protected abstract void UpdatePrice();
        protected abstract void UpdateValue();
        protected abstract void Upgrade();
        protected abstract void IncreasePrice();
        protected abstract bool CheckUpperLimit();

        public void Construct(HeroData heroData, PriceListData priceList,
            IWallet wallet, Hero hero, ISDKService sdk)
        {
            _sdk = sdk;
            _hero = hero;
            _wallet = wallet;
            PriceList = priceList;
            HeroData = heroData;

            UpdatePrice();
            UpdateValue();

            _wallet.MoneyChanged += SetConfigurationPrice;
        }

        protected override void OnDisabled() => 
            _wallet.MoneyChanged -= SetConfigurationPrice;

        private void SetConfigurationPrice(int moneyAmount)
        {
            if (CurrentPrice <= moneyAmount)
            {
                Ad.SetActive(false);
                PriceView.gameObject.SetActive(true);
            }
            else
            {
                Ad.SetActive(true);
                PriceView.gameObject.SetActive(false);
            }
        }
        
        public void MakeFree()
        {
            _tempPrice = CurrentPrice;

            CurrentPrice = 0;
            PriceView.text = FreeLot;
        }

        public void Purchase()
        {
            if (_wallet.Check(CurrentPrice))
            {
                CurrentValue++;
                Upgrade();
                UpdateValue();

                IncreasePrice();
                UpdatePrice();

                _hero.Upgrade();

                _wallet.SpendMoney(CurrentPrice);
            }
            else
            {
                _sdk.AdReward(delegate
                {
                    CurrentValue++;
                    Upgrade();
                    UpdateValue();
                    
                    _hero.Upgrade();
                });
            }
            
            if (CurrentPrice == 0) 
                CurrentPrice = _tempPrice;
            
            UpdateValueView();
            UpdatePriceView();

            SetConfigurationValue();
        }

        private void SetConfigurationValue()
        {
            if (CheckUpperLimit())
            {
                MaxLevel.SetActive(true);
                ReadyUpgrade.SetActive(false);
            }
            else
            {
                MaxLevel.SetActive(false);
                ReadyUpgrade.SetActive(true);
            }
        }

        private void UpdateValueView() => 
            ValueView.text = CurrentValue.ToString();

        private void UpdatePriceView() => 
            PriceView.text = CurrentPrice.ToString();
    }
}