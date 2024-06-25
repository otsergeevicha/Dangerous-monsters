using System;
using Modules;
using Plugins.MonoCache;
using UnityEngine;

namespace Loots
{
    public class MedicalBox : MonoCache, ILoot
    {
        [Header("Animators")] 
        [SerializeField] private Animator _animatorBody;
        [SerializeField] private Animator _animatorCap;
        
        public void OnActive()
        {
            gameObject.SetActive(true);
            
            _animatorBody.enabled = true;
            _animatorCap.enabled = false;
            
                /*
                 * 1. включиться
                 * 2. после реварда анимация открытия
                 * 3. оверлап и вознаграждение героя
                 * 4. анимация отключения
                 * 5. оповещение о вознаграждение
                 * 6. выключение
                 */
        }

        public void Open(Action opened)
        {
            opened?.Invoke();
            _animatorBody.enabled = false;
            _animatorCap.enabled = true;
        }

        public void InActive()
        {
            _animatorBody.enabled = true;
            _animatorCap.enabled = false;
            
            gameObject.SetActive(false);
        }
    }
}