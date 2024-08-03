using System;
using System.Collections;
using Enemies;
using Plugins.MonoCache;
using UnityEngine;

namespace ContactZones
{
    public class MonstersPortal : MonoCache
    {
        [SerializeField] private Light _light;

        private const float MaxIntensity = 40f;
        private Coroutine _coroutine;
        private float _currentIntensity;

        public event Action OnEscaped;

        private void Start() => 
            _currentIntensity = _light.intensity;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.Escape();
                OnEscaped?.Invoke();
                
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(UpdateView());
            }
        }
        
        private IEnumerator UpdateView()
        {
            while (!Mathf.Approximately(_light.intensity, MaxIntensity))
            {
                _light.intensity = Mathf.MoveTowards(_light.intensity, MaxIntensity, Time.deltaTime * 2f);
                yield return null;
            }

            while (!Mathf.Approximately(_light.intensity, _currentIntensity))
            {
                _light.intensity = Mathf.MoveTowards(_light.intensity, _currentIntensity, Time.deltaTime * 2f);
                yield return null;
            }
        }
    }
}