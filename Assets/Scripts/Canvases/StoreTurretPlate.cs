using System;
using System.Linq;
using Infrastructure.Factory.Pools;
using Player;
using Plugins.MonoCache;
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
        [SerializeField] private Image _iconAdd;
        [SerializeField] private Image _iconTurret;
        
        [Header("Second")]
        [SerializeField] private GameObject _iconUpgrade;

        private readonly float _waitTime = 2f;
        
        private bool _isWaiting;
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
                
                _iconAdd.gameObject.SetActive(false);
                _iconUpgrade.gameObject.SetActive(true);
            }
        }

        private void ResetFill() => 
            _background.fillAmount = 1;
    }
}