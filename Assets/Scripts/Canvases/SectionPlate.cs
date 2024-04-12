using System;
using Player;
using Plugins.MonoCache;
using Services.Bank;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class SectionPlate : MonoCache
    {
        [SerializeField] private GameObject _section;
        [SerializeField] private GameObject[] _inActiveObjects;
        
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _price;
        
        private readonly float _waitTime = 2f;
        
        private IWallet _wallet;
        private PriceListData _priceList;
        
        private bool _isWaiting;
        private float _currentFillAmount = 1f;
        private PoolData _poolData;

        public void Construct(IWallet wallet, PriceListData priceListData, PoolData poolData)
        {
            _poolData = poolData;
            _priceList = priceListData;
            _wallet = wallet;

            UpdatePrice();
        }

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
                }
            }
        }

        private void FinishWaiting()
        {
            int currentPrice = _poolData.CurrentLevelGame * _priceList.SectionPriceMultiplier;
            
            if (_wallet.Check(currentPrice))
            {
                _wallet.SpendMoney(currentPrice);
                OnAdditionalSection();
                gameObject.SetActive(false);
            }
        }

        private void OnAdditionalSection()
        {
            _section.SetActive(true);

            foreach (GameObject inActiveObject in _inActiveObjects) 
                inActiveObject.SetActive(false);
        }

        private void UpdatePrice() => 
            _price.text = $"{_poolData.CurrentLevelGame * _priceList.SectionPriceMultiplier}";

        private void ResetFill() => 
            _background.fillAmount = 1;
    }
}