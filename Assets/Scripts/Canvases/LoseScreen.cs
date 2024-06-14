using System;
using Plugins.MonoCache;
using UnityEngine;

namespace Canvases
{
    public class LoseScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;

        public event Action OnClickReStart;
        
        private void Start() => 
            _canvas.enabled = false;

        private void OnValidate() => 
            _canvas ??= Get<Canvas>();

        public void OnActive()
        {
            _canvas.enabled = true;
            SetCursorState(false);
        }

        public void InActive()
        {
            OnClickReStart?.Invoke();
            _canvas.enabled = false;
            
            SetCursorState(true);
        }
        
        private void SetCursorState(bool newState) =>
            Cursor.lockState = newState
                ? CursorLockMode.Locked
                : CursorLockMode.None;
    }
}