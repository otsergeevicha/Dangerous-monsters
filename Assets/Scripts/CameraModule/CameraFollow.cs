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

        private readonly bool _cursorLocked = true;
        private bool _zoom;

        public void Construct(Transform cameraRoot)
        {
            _cameraFollow.Follow = cameraRoot;
            _zoomFollow.Follow = cameraRoot;
            
            _cameraFollow.LookAt = cameraRoot;
            _zoomFollow.LookAt = cameraRoot;
            
            SetCursorState(_cursorLocked);
        }

        public Camera GetCameraMain() =>
            _camera;

        private void SetCursorState(bool newState) =>
            Cursor.lockState = newState
                ? CursorLockMode.Locked
                : CursorLockMode.None;
    }
}