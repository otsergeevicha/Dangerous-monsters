using Cinemachine;
using Plugins.MonoCache;
using UnityEngine;

namespace CameraModule
{
    public class CameraFollow : MonoCache
    {
        [SerializeField] private AudioListener _audioListener;
        
        [SerializeField] private CinemachineVirtualCamera _cameraFollow;
        [SerializeField] private CinemachineVirtualCamera _zoomFollow;
        [SerializeField] private CinemachineVirtualCamera _markerCamera;
        
        [SerializeField] private Camera _camera;
        
        private readonly float _shakeIntensity = 1.5f;
        private readonly float _shakeTime = .2f;
        private readonly float _showMarkerTime = 2f;
        
        private CinemachineBasicMultiChannelPerlin _perlin;
        private float _timer;
        private bool _isShake;
        private bool _isShowMarker;
        private bool _isZoom;

        public void Construct(Transform cameraRoot)
        {
            _cameraFollow.Follow = cameraRoot;
            _zoomFollow.Follow = cameraRoot;
            
            _cameraFollow.LookAt = cameraRoot;
            _zoomFollow.LookAt = cameraRoot;

            _perlin = _zoomFollow.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); 
            StopShake();
        }

        protected override void UpdateCached()
        {
            if (_isShake)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0) 
                    StopShake();
            }

            if (_isShowMarker)
            {
                _timer -= Time.deltaTime;
                
                if (_timer <= 0) 
                    StopShowMarker();
            }
        }

        public AudioListener GetListener =>
            _audioListener;

        public void Shake()
        {
            _isShake = true;
            _perlin.m_AmplitudeGain = _shakeIntensity;
            _timer = _shakeTime;
        }

        public void OnZoom()
        {
            _isZoom = true;
            _zoomFollow.gameObject.SetActive(true);
            _cameraFollow.gameObject.SetActive(false);
        }

        public void OffZoom()
        {
            _isZoom = false;
            _cameraFollow.gameObject.SetActive(true);
            _zoomFollow.gameObject.SetActive(false);
        }

        public Camera GetCameraMain =>
            _camera;

        private void StopShake()
        {
            _isShake = false;
            _perlin.m_AmplitudeGain = 0f;
            _timer = 0;
        }

        public void ShowMarker(Transform rootCamera)
        {
            
        }

        private void StopShowMarker()
        {
            throw new System.NotImplementedException();
        }
    }
}