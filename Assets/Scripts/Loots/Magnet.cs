using System;
using Agava.YandexGames;
using Player;
using Plugins.MonoCache;

namespace Loots
{
    public class Magnet : MonoCache, ILoot
    {
        private const string EngName = "magnet";
        private const string RuName = "магнит";
        
        private Hero _hero;

        public void Construct(Hero hero) => 
            _hero = hero;

        public void OnActive() => 
            gameObject.SetActive(true);

        public void Open(Action opened)
        {
            _hero.OnMagnetEffect();
            opened?.Invoke();
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

        public void InActive() => 
            gameObject.SetActive(false);
    }
}