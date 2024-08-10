using CameraModule;
using Markers;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class BaseGate : MonoCache
    {
        private HeroAimRing _heroAimRing;
        private bool _heroOnBase = true;
        private CameraFollow _cameraFollow;
        private Hero _hero;

        public void Construct(HeroAimRing heroAimRing, CameraFollow cameraFollow, Hero hero)
        {
            _hero = hero;
            _cameraFollow = cameraFollow;
            _heroAimRing = heroAimRing;
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero _)) 
                UpdateState();
        }

        private void UpdateState()
        {
            _heroOnBase = !_heroOnBase;
            _hero.AnimationController.SetActualRunHash(_heroOnBase);
            _hero.SetShootingState(_heroOnBase);

            if (!_heroOnBase)
            {
                _heroAimRing.OnActive();
                _cameraFollow.OnZoom();
            }
            else
            {
                _heroAimRing.InActive();
                _cameraFollow.OffZoom();
            }
        }

        public void UpdateLevel() => 
            UpdateState();
    }
}