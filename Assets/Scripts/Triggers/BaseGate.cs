﻿using CameraModule;
using Markers;
using Modules;
using Player;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Triggers
{
    public class BaseGate : MonoCache
    {
        [SerializeField] private TriggerOnZone _triggerOnZone;
        [SerializeField] private TriggerOnBase _triggerOnBase;

        [SerializeField] private ParticleSystem _explosionVfx;
        [SerializeField] private ParticleSystem _gateZoneVfx;
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private BoxCollider _collider;

        [SerializeField] private Transform _rootMarker;

        public Transform AgroPoint;
        
        private HeroAimRing _heroAimRing;
        private CameraFollow _cameraFollow;
        private Hero _hero;
        private HealthGateModule _healthGateModule;

        public void Construct(HeroAimRing heroAimRing, CameraFollow cameraFollow, Hero hero, PoolData poolData)
        {
            _hero = hero;
            _cameraFollow = cameraFollow;
            _heroAimRing = heroAimRing;

            _healthGateModule = new HealthGateModule(poolData.MaxHealthBaseGate);

            _triggerOnZone.OnEntered += OnZone;
            _triggerOnBase.OnEntered += OnBase;

            _healthGateModule.OnGateFall += InActive;
            
            OnActive();
        }

        protected override void OnDisabled()
        {
            _triggerOnZone.OnEntered -= OnZone;
            _triggerOnBase.OnEntered -= OnBase;
            
            _healthGateModule.OnGateFall -= InActive;
        }

        public void UpdateLevel()
        {
            OnActive();
            OnBase();
        }

        public void TakeDamage() => 
            _healthGateModule.ApplyDamage();

        private void OnZone()
        {
            _hero.AnimationController.SetActualRunHash(false);
            _hero.SetShootingState(false);
            _hero.BasketClear();
            
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

        private void InActive()
        {
            _cameraFollow.ShowMarker(_rootMarker);
            _mesh.enabled = false;
            _collider.enabled = false;
            
            Invoke(nameof(Explosion), .4f);
        }

        public void OnActive()
        {
            _mesh.enabled = true;
            _collider.enabled = true;
            _explosionVfx.gameObject.SetActive(false);
            _gateZoneVfx.gameObject.SetActive(true);
        }

        private void Explosion()
        {
            _explosionVfx.gameObject.SetActive(true);
            _gateZoneVfx.gameObject.SetActive(false);
        }
    }
}