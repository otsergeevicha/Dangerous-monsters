using CameraModule;
using Markers;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class BaseGate : MonoCache
    {
        [SerializeField] private TriggerOnZone _triggerOnZone;
        [SerializeField] private TriggerOnBase _triggerOnBase;
        
        private HeroAimRing _heroAimRing;
        private CameraFollow _cameraFollow;
        private Hero _hero;

        public void Construct(HeroAimRing heroAimRing, CameraFollow cameraFollow, Hero hero)
        {
            _hero = hero;
            _cameraFollow = cameraFollow;
            _heroAimRing = heroAimRing;

            _triggerOnZone.OnEntered += OnZone;
            _triggerOnBase.OnEntered += OnBase;
        }

        protected override void OnDisabled()
        {
            _triggerOnZone.OnEntered -= OnZone;
            _triggerOnBase.OnEntered -= OnBase;
        }

        private void OnZone()
        {
            _hero.AnimationController.SetActualRunHash(false);
            _hero.SetShootingState(false);
            
            _heroAimRing.OnActive();
            _cameraFollow.OnZoom();
        }

        private void OnBase()
        {
            _hero.AnimationController.SetActualRunHash(true);
            _hero.SetShootingState(true);
            
            _heroAimRing.InActive();
            _cameraFollow.OffZoom();
        }

        public void UpdateLevel() =>
            OnBase();
    }
}