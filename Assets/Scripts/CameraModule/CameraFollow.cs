using Cinemachine;
using Plugins.MonoCache;
using Services.Inputs;
using UnityEngine;

namespace CameraModule
{
    public class CameraFollow : MonoCache
    {
        [SerializeField] private CinemachineVirtualCamera _cameraFollow;
        [SerializeField] private CinemachineVirtualCamera _zoomFollow;

        [SerializeField] private Camera _camera;

        private readonly bool _cursorLocked = true;

        private bool _zoom;
    
        public void Construct(IInputService input, Transform cameraRootHero)
        {
            _cameraFollow.Follow = cameraRootHero;
            _zoomFollow.Follow = cameraRootHero;
            
            _zoomFollow.gameObject.SetActive(false);
            SetCursorState(_cursorLocked);
        }

        public Camera GetCameraMain() =>
            _camera;

        private void OnZoom()
        {
            if (_zoom)
            {
                _zoomFollow.gameObject.SetActive(false);
                _cameraFollow.gameObject.SetActive(true);
                _zoom = false;
            }
            else
            {
                _zoomFollow.gameObject.SetActive(true);
                _cameraFollow.gameObject.SetActive(false);
                _zoom = true;
            }
        }

        private void SetCursorState(bool newState) =>
            Cursor.lockState = newState
                ? CursorLockMode.Locked
                : CursorLockMode.None;
    }
}