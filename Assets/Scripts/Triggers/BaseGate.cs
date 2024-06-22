using CameraModule;
using Player;
using Plugins.MonoCache;
using RingZone;
using UnityEngine;

namespace Triggers
{
    public class BaseGate : MonoCache
    {
        private HeroAimRing _heroAimRing;
        private bool _heroOnBase = true;
        private CameraFollow _cameraFollow;

        public void Construct(HeroAimRing heroAimRing, CameraFollow cameraFollow)
        {
            _cameraFollow = cameraFollow;
            _heroAimRing = heroAimRing;
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Hero hero))
            {
                _heroOnBase = !_heroOnBase;
                hero.AnimationController.SetActualRunHash(_heroOnBase);
                hero.SetShootingState(_heroOnBase);
                
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
        }
    }
}