using CameraModule;
using Plugins.MonoCache;
using Services.Inputs;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HeroMovement))]
    public class Hero : MonoCache
    {
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private RootCamera _rootCamera;

        public void Construct(IInputService input) => 
            _heroMovement.Construct(input);

        private void OnValidate() => 
            _heroMovement = Get<HeroMovement>();
        
        public Transform GetCameraRoot() =>
            _rootCamera.transform;
    }
}

public class ZoneAmmo : MonoCache
{
    
}