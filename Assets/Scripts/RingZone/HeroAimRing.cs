using Plugins.MonoCache;
using UnityEngine;

namespace RingZone
{
    public class HeroAimRing : MonoCache
    {
        [SerializeField] private ParticleSystem _particle;
        
        private Transform _followingTransform;
        private float _heroDataRadiusDetection;

        public void Construct(Transform heroTransform, float heroDataRadiusDetection)
        {
            _heroDataRadiusDetection = heroDataRadiusDetection/2;
            _followingTransform = heroTransform;
            InActive();
        }

        protected override void UpdateCached()
        {
            if (!isActiveAndEnabled)
                return;

            _particle.startSize = _heroDataRadiusDetection;
            
            transform.position =
                new Vector3(_followingTransform.position.x, 1f,
                    _followingTransform.position.z);
        }

        public void OnActive() => 
            gameObject.SetActive(true);

        public void InActive() => 
            gameObject.SetActive(false);
    }
}