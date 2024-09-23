using Plugins.MonoCache;
using Agava.YandexGames;
using Services.Bank;
using Services.SDK;
using TMPro;
using UnityEngine;

namespace Canvases
{
    public class WinScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _buttonContinue;

        [SerializeField] private TMP_Text _remainingMoney;
        [SerializeField] private TMP_Text _remainingGem;

        private const string EngDescription = "x2 coins and crystals";
        private const string RuDescription = "x2 монеты и кристалы";
        
        private IWallet _wallet;
        private ISDKService _sdk;
        private bool _isAdShowed;
        private NotifyRewardScreen _hudNotifyRewardScreen;

        public void Construct(IWallet wallet, ISDKService sdk, NotifyRewardScreen hudNotifyRewardScreen)
        {
            _hudNotifyRewardScreen = hudNotifyRewardScreen;
            _sdk = sdk;
            _wallet = wallet;
        }

        private void Start()
        {
            _canvas.enabled = false;
            _buttonContinue.SetActive(false);
        }

        private void OnValidate() =>
            _canvas ??= Get<Canvas>();

        public void RewardX2()
        {
            _hudNotifyRewardScreen.OnActive(BuildDescription());
            _hudNotifyRewardScreen.RewardCompleted += OnRewardCompleted;
        }

        public void OnActive()
        {
            Time.timeScale = 0;
            _canvas.enabled = true;

            _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
            _remainingGem.text = _wallet.ReadCurrentGem().ToString();
        }

        public void InActive()
        {
            if (_isAdShowed == false)
                _sdk.ShowInterstitial(ContinueGame);
            else
                ContinueGame();

            _isAdShowed = false;
        }

        private void OnRewardCompleted()
        {
            _wallet.ApplyMoney(_wallet.ReadCurrentMoney());
            _wallet.ApplyGem(_wallet.ReadCurrentGem());

            _remainingMoney.text = _wallet.ReadCurrentMoney().ToString();
            _remainingGem.text = _wallet.ReadCurrentGem().ToString();

            _isAdShowed = true;
            _hudNotifyRewardScreen.RewardCompleted -= OnRewardCompleted;
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
        
        private void ContinueGame()
        {
            Time.timeScale = 1;
            _buttonContinue.SetActive(false);
            _canvas.enabled = false;
        }

        public void ActiveButtonContinue() =>
            _buttonContinue.SetActive(true);
    }
}