using Cinemachine;
using Plugins.MonoCache;
using UnityEngine;

namespace CameraModule
{
    public class CameraFollow : MonoCache
    {
        [SerializeField] private CinemachineVirtualCamera _cameraFollow;
        [SerializeField] private CinemachineVirtualCamera _zoomFollow;
        [SerializeField] private Camera _camera;

        public void Construct(Transform cameraRoot)
        {
            _cameraFollow.Follow = cameraRoot;
            _zoomFollow.Follow = cameraRoot;
            
            _cameraFollow.LookAt = cameraRoot;
            _zoomFollow.LookAt = cameraRoot;
        }

        public void OnZoom()
        {
            _zoomFollow.gameObject.SetActive(true);
            _cameraFollow.gameObject.SetActive(false);
        }
        
        public void OffZoom()
        {
            _cameraFollow.gameObject.SetActive(true);
            _zoomFollow.gameObject.SetActive(false);
        }
        
        public Camera GetCameraMain =>
            _camera;
    }
}