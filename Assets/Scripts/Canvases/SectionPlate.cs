using System;
using Agava.YandexGames;
using Player;
using Plugins.MonoCache;
using Services.Bank;
using Services.SaveLoad;
using Services.SDK;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    enum TypeSection
    {
        LeftSection,
        RightSection
    }
    
    public class SectionPlate : MonoCache
    {
        [Range((int)TypeSection.LeftSection, (int)TypeSection.RightSection)]
        [SerializeField] private int _indexSection;
        
        [SerializeField] private GameObject _section;
        [SerializeField] private GameObject[] _inActiveObjects;

        [SerializeField] private Image _background;
        
        [SerializeField] private TMP_Text _price;
        [SerializeField] private GameObject _ad;
        
        private const string EngDescription = "expanding the base";
        private const string RuDescription = "расширение базы";
        
        private readonly float _waitTime = 2f;

        private IWallet _wallet;
        private PriceListData _priceList;

        private bool _isWaiting;
        private float _currentFillAmount = 1f;
        private PoolData _poolData;
        private int _currentPrice;
        private NotifyRewardScreen _hudNotifyRewardScreen;
        private ISave _save;

        public event Action OnNotifyAssistant;

        public void Construct(IWallet wallet, PriceListData priceListData, PoolData poolData,
            NotifyRewardScreen hudNotifyRewardScreen, ISave save)
        {
            _save = save;
            _hudNotifyRewardScreen = hudNotifyRewardScreen;
            _poolData = poolData;
            _priceList = priceListData;
            _wallet = wallet;

            _currentPrice = _poolData.CurrentLevelGame * _priceList.SectionPriceMultiplier;
            
            UpdatePrice();
            _wallet.MoneyChanged += SetConfigurationPrice;
            SetConfigurationPrice(_wallet.ReadCurrentMoney());

            CorrectState();
        }

        protected override void OnDisabled() => 
            _wallet.MoneyChanged -= SetConfigurationPrice;

        private void OnTriggerEnter(Collider collision)
        {
            ResetFill();

            if (collision.TryGetComponent(out Hero _))
                _isWaiting = true;
        }

        private void OnTriggerExit(Collider collision)
        {
            ResetFill();

            if (collision.TryGetComponent(out Hero _))
            {
                _isWaiting = false;
                _currentFillAmount = 1;
                ResetFill();
            }
        }

        protected override void UpdateCached()
        {
            if (_isWaiting)
            {
                _currentFillAmount -= Time.deltaTime / _waitTime;
                _background.fillAmount = _currentFillAmount;

                if (_currentFillAmount <= Single.Epsilon)
                {
                    FinishWaiting();
                    _isWaiting = false;
                    _currentFillAmount = 1f;
                    ResetFill();
                }
            }
        }

        public void UpdateLevel()
        {
            _section.SetActive(false);

            foreach (GameObject inActiveObject in _inActiveObjects)
                inActiveObject.SetActive(true);

            _currentPrice = _poolData.CurrentLevelGame * _priceList.SectionPriceMultiplier;
            
            UpdatePrice();
            SetConfigurationPrice(_wallet.ReadCurrentMoney());
            
            SaveOpen(false);
        }

        private void FinishWaiting()
        {
            if (_wallet.Check(_currentPrice))
            {
                SaveOpen(true);

                _wallet.SpendMoney(_currentPrice);
                OnAdditionalSection();
                gameObject.SetActive(false);
            }
            else
            {
                _hudNotifyRewardScreen.OnActive(BuildDescription());

                _hudNotifyRewardScreen.RewardCompleted += () =>
                {
                    SaveOpen(true);
                    
                    OnAdditionalSection();
                    gameObject.SetActive(false);
                };
            }
        }

        private void CorrectState()
        {
            if (_save.AccessProgress().DataStateLevel.OpenLeftSection
                && _indexSection == (int)TypeSection.LeftSection)
            {
                OnAdditionalSection();
                gameObject.SetActive(false);
            }

            if (_save.AccessProgress().DataStateLevel.OpenRightSection
                && _indexSection == (int)TypeSection.RightSection)
            {
                OnAdditionalSection();
                gameObject.SetActive(false);
            }
        }

        private void SaveOpen(bool flag)
        {
            if (_indexSection == (int)TypeSection.LeftSection)
                _save.AccessProgress().DataStateLevel.OpenLeftSection = flag;
            else
                _save.AccessProgress().DataStateLevel.OpenRightSection = flag;
            
            _save.Save();
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
        
        private void SetConfigurationPrice(int moneyAmount)
        {
            if (_currentPrice <= moneyAmount)
            {
                _ad.SetActive(false);
                _price.gameObject.SetActive(true);
            }
            else
            {
                _ad.SetActive(true);
                _price.gameObject.SetActive(false);
            }
        }

        private void OnAdditionalSection()
        {
            OnNotifyAssistant?.Invoke();
            
            _section.SetActive(true);

            foreach (GameObject inActiveObject in _inActiveObjects)
                inActiveObject.SetActive(false);
        }

        private void UpdatePrice() =>
            _price.text = $"{_currentPrice}";

        private void ResetFill() =>
            _background.fillAmount = 1;
    }
}