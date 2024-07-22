using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Player.Animation
{
    public class HeroAnimation : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;

        private HeroData _heroData;
        private int _runHash;
        private bool _heroOnBase = true;

        public void Construct(HeroData heroData)
        {
            _heroData = heroData;
            _runHash = _heroData.RunHash;
        }

        private void OnValidate() => 
            _animator ??= Get<Animator>();

        public void SetActualRunHash(bool heroOnBase)
        {
            _heroOnBase = heroOnBase;
            
            _animator.SetBool(_runHash, false);
            _runHash = heroOnBase ? _heroData.RunHash : _heroData.RunGunHash;
        }

        public void EnableRun()
        {
            _animator.SetBool(_heroData.IdleAimingHash, false);
            _animator.SetBool(_runHash, true);
        }

        public void EnableIdle()
        {
            if (!_heroOnBase) 
                _animator.SetBool(_heroData.IdleAimingHash, true);
            
            if (_heroOnBase) 
                _animator.SetBool(_runHash, false);
        }

        // public void EnableShoot() => 
        //     _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime));
        //
        // public void DisableShoot() => 
        //     _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, Time.deltaTime));
    }
}