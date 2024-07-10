using System;
using Plugins.MonoCache;
using UnityEngine;

namespace Canvases
{
    public class StartScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;

        public event Action OnClickStart;
        
        private void OnValidate() => 
            _canvas ??= Get<Canvas>();
        
        public void OnActive()
        {
            Time.timeScale = 0;
            _canvas.enabled = true;
        }
        
        public void InActive()
        {
            _canvas.enabled = false;
            OnClickStart?.Invoke();
            Time.timeScale = 1;
        }
    }
}