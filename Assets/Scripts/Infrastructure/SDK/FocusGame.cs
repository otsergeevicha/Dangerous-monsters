using Agava.WebUtility;
using Plugins.MonoCache;
using UnityEngine;

namespace Infrastructure.SDK
{
    public class FocusGame : MonoCache
    {
        private AudioListener _listener;

#if !UNITY_EDITOR

        public void Construct(AudioListener listener)
        {
            _listener = listener;
            
            Application.focusChanged += OnfocusChanged;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeEvent;
        }

        protected override void OnDisabled()
        {
            Application.focusChanged -= OnfocusChanged;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeEvent;
        }

        private void OnfocusChanged(bool inApp)
        {
            MuteAudio(!inApp);
            PauseGame(!inApp);
        }

        private void PauseGame(bool value) =>
            Time.timeScale = value ? 0 : 1;

        private void MuteAudio(bool value) => 
            _listener.enabled = value;

        private void OnInBackgroundChangeEvent(bool isBackground)
        {
            MuteAudio(isBackground);
            PauseGame(isBackground);
        }
#endif
    }
}