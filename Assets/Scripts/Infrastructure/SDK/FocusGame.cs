using Agava.WebUtility;
using Plugins.MonoCache;
using UnityEngine;

namespace Infrastructure.SDK
{
    public class FocusGame : MonoCache
    {
        private AudioSource _audioSource;

        public void Construct(AudioSource audioSource)
        {
            _audioSource = audioSource;

            Application.focusChanged += OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeEvent;
        }

        protected override void OnDisabled()
        {
            Application.focusChanged -= OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeEvent;
        }

        private void OnInBackgroundChangeApp(bool inApp)
        {
            if (inApp) 
                Mute();
            
            if (!inApp) 
                UnMute();
        }

        private void OnInBackgroundChangeEvent(bool isBackground)
        {
            if (isBackground) 
                Mute();
            
            if (!isBackground) 
                UnMute();
        }
        
        private void Mute()
        {
            Time.timeScale = 0;
            _audioSource.volume = 0;
        }

        private void UnMute()
        {
            Time.timeScale = 1;
            _audioSource.volume = 1;
        }
    }
}