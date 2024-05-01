using Plugins.MonoCache;
using UnityEngine;

namespace RingZone
{
    public class HeroAimRing : MonoCache
    {
        [SerializeField] private ParticleSystem _mainParticle;
        [SerializeField] private ParticleSystem _floorRing;
        [SerializeField] private ParticleSystem _magicGlow;
        [SerializeField] private ParticleSystem AOESmile;
        
        private Transform _followingTransform;

        public void Construct(Transform heroTransform)
        {
            _followingTransform = heroTransform;
            InActive();
        }

        protected override void UpdateCached()
        {
            if (!isActiveAndEnabled)
                return;

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