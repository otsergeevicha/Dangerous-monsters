using System;
using Plugins.MonoCache;
using UnityEngine;

namespace Canvases
{
    public class LoseScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;
        [SerializeField] private ParticleSystem _particle;

        public event Action OnClickReStart;
        
        private void Start()
        {
            _canvas.enabled = false;
            _particle.gameObject.SetActive(false);
        }

        private void OnValidate() => 
            _canvas ??= Get<Canvas>();

        public void OnActive()
        {
            Time.timeScale = 0;
            _particle.gameObject.SetActive(true);
            SetCursorState(false);
        }

        public void InActive()
        {
            OnClickReStart?.Invoke();
            Time.timeScale = 1;
            _particle.gameObject.SetActive(false);
            
            SetCursorState(true);
        }
        
        private void SetCursorState(bool newState) =>
            Cursor.lockState = newState
                ? CursorLockMode.Locked
                : CursorLockMode.None;
    }
}