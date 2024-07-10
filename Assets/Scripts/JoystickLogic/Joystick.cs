using Plugins.MonoCache;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace JoystickLogic
{
    public enum VirtualJoystickType
    {
        Fixed,
        Floating
    }
    
    public class Joystick : MonoCache
    {
        [HideInInspector] [SerializeField] private CanvasGroup _bgCanvasGroup;
        [HideInInspector] [SerializeField] private Canvas _canvas;
        [HideInInspector] [SerializeField] private RectTransform _baseRect;
        [HideInInspector] [SerializeField] private OnScreenStick _handleStickController;
        
        [SerializeField] private RectTransform _centerArea;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private bool _hideOnPointerUp;
        [SerializeField] private bool _centralizeOnPointerUp = true;

        private readonly VirtualJoystickType _joystickType = VirtualJoystickType.Floating;
        private Vector2 _initialPosition = Vector2.zero;
        private Camera _camera;

        protected override void OnEnabled()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += OnDown;
            ETouch.Touch.onFingerUp += OnUp;
        }

        protected override void OnDisabled()
        {
            ETouch.Touch.onFingerDown -= OnDown;
            ETouch.Touch.onFingerUp -= OnUp;
            EnhancedTouchSupport.Disable();
        }

        private void Awake()
        {
            Vector2 center = new Vector2(0.5f, 0.5f);
            
            _centerArea.pivot = center;
            _handle.anchorMin = center;
            _handle.anchorMax = center;
            _handle.pivot = center;
            _handle.anchoredPosition = Vector2.zero;

            _initialPosition = _centerArea.anchoredPosition;

            if (_joystickType == VirtualJoystickType.Fixed)
            {
                _centerArea.anchoredPosition = _initialPosition;
                _bgCanvasGroup.alpha = 1;
            }
            else if (_joystickType == VirtualJoystickType.Floating)
                _bgCanvasGroup.alpha = _hideOnPointerUp ? 0 : 1;
        }

        private void OnValidate()
        {
            _canvas ??= GetComponentInParent<Canvas>();
            _baseRect ??= GetComponent<RectTransform>();
            _bgCanvasGroup ??= _centerArea.GetComponent<CanvasGroup>();
            _handleStickController ??= _handle.gameObject.GetComponent<OnScreenStick>();
        }

        public void OnDown(Finger touchedFinger)
        {
            if (_joystickType == VirtualJoystickType.Floating)
            {
                _centerArea.anchoredPosition = GetAnchoredPosition(touchedFinger.screenPosition);
                
                if (_hideOnPointerUp)
                    _bgCanvasGroup.alpha = 1;
            }
        }

        public void OnUp(Finger _)
        {
            if (_joystickType == VirtualJoystickType.Floating)
            {
                if (_centralizeOnPointerUp)
                    _centerArea.anchoredPosition = _initialPosition;

                _bgCanvasGroup.alpha = _hideOnPointerUp ? 0 : 1;
            }
        }

        public void InjectCamera(Camera mainCamera) =>
            _camera = mainCamera;

        private Vector2 GetAnchoredPosition(Vector2 screenPosition)
        {
            _camera = (_canvas.renderMode == RenderMode.ScreenSpaceCamera) ? _canvas.worldCamera : null;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _camera, out Vector2 localPoint))
            {
                Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
                return localPoint - (_centerArea.anchorMax * _baseRect.sizeDelta) + pivotOffset;
            }

            return Vector2.zero;
        }
    }
}