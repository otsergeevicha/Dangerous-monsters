using System;
using Agava.YandexGames;
using Plugins.MonoCache;
using Services.Bank;
using Services.SDK;
using SO;
using Spawners;
using TMPro;
using Triggers;
using UnityEngine;

namespace Canvases
{
    public class LoseScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;

        [SerializeField] private TMP_Text _remainingMoney;
        [SerializeField] private TMP_Text _remainingGem;

        [SerializeField] private TMP_Text _bonusMoney;
        [SerializeField] private TMP_Text _bonusGem;

        private const string EngDescription = "coins and crystals";
        private const string RuDescription = "монеты и кристалы";
        
        private IWallet _wallet;
        private ISDKService _sdk;
        private bool _isAdShowed;

        private PriceListData _priceList;
        private BaseGate _baseGate;
        private EnemySpawner _enemySpawner;
        private NotifyRewardScreen _hudNotifyRewardScreen;

        public event Action OnClickReStart;
        
        public void Construct(IWallet wallet, ISDKService sdk, PriceListData priceList, BaseGate baseGate,
            EnemySpawner enemySpawner, NotifyRewardScreen hudNotifyRewardScreen)
        {
            _hudNotifyRewardScreen = hudNotifyRewardScreen;
            _enemySpawner = enemySpawner;
            _baseGate = baseGate;
            _priceList = priceList;
            _sdk = sdk;
            _wallet = wallet;
        }

        private void Start() => 
            _canvas.enabled = false;

        private void OnValidate() =>
            _canvas ??= Get<Canvas>();


        public void RewardBonus()
        {
            _hudNotifyRewardScreen.OnActive(BuildDescription());
            
            _hudNotifyRewardScreen.RewardCompleted += () =>
            {
                _wallet.ApplyMoney(_priceList.LoseBonusMoney);
                _wallet.ApplyGem(_priceList.LoseBonusGem);

                _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
                _remainingGem.text = _wallet.ReadCurrentGem().ToString();

                _isAdShowed = true;
            };
        }

        private string BuildDescription()
        {
#if !UNITY_EDITOR
            return YandexGamesSdk.Environment.i18n.lang == "en"
                ? EngDescription
                : RuDescription;
#endif
            return RuDescription;
        }

        public void OnActive()
        {
            Time.timeScale = 0;
            _canvas.enabled = true;

            _bonusMoney.text = _priceList.LoseBonusMoney.ToString();
            _bonusGem.text = _priceList.LoseBonusGem.ToString();

            _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
            _remainingGem.text = _wallet.ReadCurrentGem().ToString();

            _enemySpawner.ClearField();
        }

        public void InActive()
        {
            if (_isAdShowed == false)
            {
                _sdk.ShowInterstitial(TryAgain);
                return;
            }

            if (_isAdShowed)
            {
                TryAgain();
                _isAdShowed = false;
            }
        }

        private void TryAgain()
        {
            OnClickReStart?.Invoke();
            Time.timeScale = 1;
            _canvas.enabled = false;

            _baseGate.OnActive();
            _enemySpawner.OnStart();
        }
    }
}