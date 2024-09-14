using System;
using Agava.YandexGames;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Loots
{
    public class MedicalBox : MonoCache, ILoot
    {
        [Header("Animators")] 
        [SerializeField] private Animator _animatorBody;
        [SerializeField] private Animator _animatorCap;

        private const string EngName = "healing";
        private const string RuName = "исцеление";
        
        private Hero _hero;

        public void Construct(Hero hero) => 
            _hero = hero;

        public void OnActive()
        {
            gameObject.SetActive(true);
            _animatorBody.enabled = true;
        }

        public void Open(Action opened)
        {
            _hero.OnHealing();
            opened?.Invoke();
            _animatorBody.enabled = false;
        }

        public string GetName()
        {
#if !UNITY_EDITOR
            return YandexGamesSdk.Environment.i18n.lang == "en" 
                ? EngName 
                : RuName;
#endif
            return RuName;
        }

        public void InActive()
        {
            _animatorBody.enabled = true;
            gameObject.SetActive(false);
        }
    }
}