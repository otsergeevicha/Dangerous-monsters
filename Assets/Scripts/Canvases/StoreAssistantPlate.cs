using System;
using System.Linq;
using Infrastructure.Factory.Pools;
using Player;
using Plugins.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class StoreAssistantPlate : MonoCache
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _slotData;
        [SerializeField] private Mannequin _mannequin;
        [SerializeField] private Transform _spawnPoint;

        private const string ColorText = "<#64b0ef>";
        private const string MaxText = "MAX";

        private int _currentCountAssistant = 0;
        private int _maxAssistant;
        private bool _isWaiting;
        private float _waitTime = 2f;
        private float _currentFillAmount = 1f;
        private PoolCargoAssistant _poolAssistant;

        public void Construct(int maxAssistant, PoolCargoAssistant poolAssistant)
        {
            _poolAssistant = poolAssistant;
            _maxAssistant = maxAssistant;

            if (_currentCountAssistant == _maxAssistant) 
                _mannequin.gameObject.SetActive(false);

            UpdateSlotText();
            ResetFill();
        }

        private void OnTriggerEnter(Collider collision)
        {
            ResetFill();
            
            if (collision.TryGetComponent(out Hero _) && _currentCountAssistant < _maxAssistant) 
                _isWaiting = true;
        }
        
        private void OnTriggerExit(Collider collision)
        {
            ResetFill();

            if (collision.TryGetComponent(out Hero _) && _currentCountAssistant < _maxAssistant)
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
                _image.fillAmount = _currentFillAmount;

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
            _currentCountAssistant++;
            
            if (_currentCountAssistant == _maxAssistant) 
                _mannequin.gameObject.SetActive(false);
            
            UpdateSlotText();

            _poolAssistant.Assistants.FirstOrDefault(assistant => assistant.isActiveAndEnabled == false)?.OnActive(_spawnPoint);
        }

        private void UpdateSlotText() =>
            _slotData.text = _currentCountAssistant != _maxAssistant 
                ? $"{_currentCountAssistant} {ColorText}/ {_maxAssistant}" 
                : $"{ColorText}{MaxText}";

        private void ResetFill() => 
            _image.fillAmount = 1;
    }
}