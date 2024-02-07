using CameraModule;
using Plugins.MonoCache;
using Services.Inputs;
using SO;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HeroMovement))]
    public class Hero : MonoCache
    {
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private RootCamera _rootCamera;
        [SerializeField] private AmmoBasket _ammoBasket;

        public void Construct(IInputService input, HeroData heroData, PoolAmmoBox pool)
        {
            _heroMovement.Construct(input, heroData.HeroSpeed, heroData.HeroIdleHash, heroData.HeroRunHash);
            _ammoBasket.Construct(pool);
        }

        private void OnValidate() =>
            _heroMovement = Get<HeroMovement>();

        public Transform GetCameraRoot() =>
            _rootCamera.transform;
    }
}