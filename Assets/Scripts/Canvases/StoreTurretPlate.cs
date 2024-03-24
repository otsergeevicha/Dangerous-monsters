using System;
using System.Linq;
using Infrastructure.Factory.Pools;
using Player;
using Plugins.MonoCache;
using TMPro;
using Turrets;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    public class StoreTurretPlate : MonoCache
    {
        [SerializeField] private Image _background;
        [SerializeField] private Transform _spawnPoint;
        
        [Header("First")]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _description;
        
        [Header("Second")]
        [SerializeField] private Image _newIcon;
        [SerializeField] private TMP_Text _newDescription;

        private bool _isWaiting;
        private float _waitTime = 2f;
        private float _currentFillAmount = 1f;
        private PoolTurrets _poolTurrets;
        private bool _purchased;
        private Turret _turret;

        public void Construct(PoolTurrets poolTurrets)
        {
            _poolTurrets = poolTurrets;
            ResetFill();
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
            if (_turret == null)
                _turret = _poolTurrets.Turrets.FirstOrDefault(turret => 
                    turret.isActiveAndEnabled == false);
            
            if (_turret != null)
            {
                if (_purchased) 
                    _turret.Upgrade();

                if (!_turret.isActiveAndEnabled)
                {
                    _turret.OnActive(_spawnPoint);
                    _purchased = true;
                }
                
                _icon.gameObject.SetActive(false);
                _newIcon.gameObject.SetActive(true);
                    
                _description.gameObject.SetActive(false);
                _newDescription.gameObject.SetActive(true);
            }
        }

        private void ResetFill() => 
            _background.fillAmount = 1;
    }
}