using System;
using Modules;
using Player;
using Plugins.MonoCache;
using Services.Bank;
using Services.SaveLoad;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContactZones
{
    enum TypeTransitionPlate
    {
        One,
        Two
    }
    
    public class TransitionPlate : MonoCache, ITutorialPlate
    {
        [Range((int)TypeTransitionPlate.One, (int)TypeTransitionPlate.Two)]
        [SerializeField] private int _indexSection;
        
        [SerializeField] private Transform _markerPosition;
        [SerializeField] private Transform _rootCamera;
        [SerializeField] private Transform _border;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _slotData;

        private const string ColorText = "<#64b0ef>";
        private const string MaxText = "MAX";

        private readonly float _waitTime = 2f;

        private int _currentCountGem;
        private int _maxGem;

        private bool _isWaiting;
        private float _currentFillAmount = 1f;
        private IWallet _wallet;
        private PriceListData _priceList;
        private ISave _save;

        public event Action OnTutorialContacted;
        
        public void Construct(IWallet wallet, PriceListData priceList, ISave save)
        {
            _save = save;
            _priceList = priceList;
            _wallet = wallet;
            _maxGem = priceList.PriceTransitionPlate;

            UpdateSlotText();
            ResetFill();
            
            CorrectState();
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
                ResetFill();
            }
        }

        protected override void UpdateCached()
        {
            if (_isWaiting)
            {
                _currentFillAmount -= Time.deltaTime / _waitTime;
                _image.fillAmount = _currentFillAmount;

                if (_currentFillAmount <= Single.Epsilon)
                {
                    FinishWaiting();
                    _isWaiting = false;
                    _currentFillAmount = 1f;
                    ResetFill();
                }
            }
        }

        public Vector3 GetPositionMarker() => 
            _markerPosition.transform.position;

        public Transform GetRootCamera() => 
            _rootCamera;

        public void UpdateLevel()
        {
            _border.gameObject.SetActive(true);
            gameObject.SetActive(true);
            _currentCountGem = 0;
            _priceList.PriceTransitionPlate += _priceList.MultiplierIncreasePrice;
            _maxGem = _priceList.PriceTransitionPlate;
            UpdateSlotText();
            
            SaveOpen(false);
        }

        private void FinishWaiting()
        {
            int haveGems = _wallet.ReadCurrentGem();
            
            if (haveGems > 0)
            {
                int requiredCountGem = _maxGem - _currentCountGem;

                if (requiredCountGem <= haveGems)
                {
                    _wallet.SpendGem(requiredCountGem);
                    _currentCountGem += requiredCountGem;
                    UpdateSlotText();
                }
                else
                {
                    _wallet.SpendGem(haveGems);
                    _currentCountGem += haveGems;
                    UpdateSlotText();
                }
            }
            else
            {
                Debug.Log("Not enough gems");
            }

            if (_currentCountGem == _maxGem)
            {
                _border.gameObject.SetActive(false);
                gameObject.SetActive(false);

                SaveOpen(true);
                
                OnTutorialContacted?.Invoke();
            }
        }

        private void SaveOpen(bool flag)
        {
            if (_indexSection == (int)TypeTransitionPlate.One)
                _save.AccessProgress().DataStateLevel.OpenOneTransition = flag;
            else
                _save.AccessProgress().DataStateLevel.OpenTwoTransition = flag;
            
            _save.Save();
        }

        private void CorrectState()
        {
            if (_save.AccessProgress().DataStateLevel.OpenOneTransition
                && _indexSection == (int)TypeTransitionPlate.One)
            {
                _border.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            
            if (_save.AccessProgress().DataStateLevel.OpenTwoTransition
                && _indexSection == (int)TypeTransitionPlate.Two)
            {
                _border.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        
        private void UpdateSlotText() =>
            _slotData.text = _currentCountGem != _maxGem
                ? $"{_currentCountGem} {ColorText}/ {_maxGem}"
                : $"{ColorText}{MaxText}";

        private void ResetFill() =>
            _image.fillAmount = 1;
    }
}