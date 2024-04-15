using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Player.Animation
{
    public class HeroAnimation : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;

        private readonly float _rotationSpeed = 5.5f;
        
        private HeroData _heroData;
        private int _runHash;

        public void Construct(HeroData heroData)
        {
            _heroData = heroData;
            _runHash = _heroData.RunHash;
        }

        private void OnValidate() => 
            _animator ??= Get<Animator>();

        public void SetActualRunHash(bool heroOnBase)
        {
            _animator.SetBool(_runHash, false);
            _runHash = heroOnBase ? _heroData.RunHash : _heroData.RunGunHash;
        }

        public void EnableRun()
        {
            _animator.SetBool(_runHash, true);
        }

        public void EnableIdle()
        {
            _animator.SetBool(_runHash, false);
        }
    }
}