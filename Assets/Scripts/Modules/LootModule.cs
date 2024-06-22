using System;
using Plugins.MonoCache;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Modules
{
    enum TypeLoot
    {
        Money,
        Gem,
        Magnet,
        Health,
        Upgrade
    }
    
    public class LootModule : MonoCache
    {
        private const int MaxInclusive = 100;
        
        [HideInInspector] [SerializeField] private Magnet _magnet;
        [HideInInspector] [SerializeField] private MedicalBox _medicalBox;
        [HideInInspector] [SerializeField] private RandomBox _randomBox;

        [SerializeField] private GameObject _markerParticle;
        
        [SerializeField] private GameObject _adView;
        
        [SerializeField] private GameObject _freeView;
        [SerializeField] private Image _confirmationBar;

        private void OnValidate()
        {
            _magnet ??= ChildrenGet<Magnet>();
            _medicalBox ??= ChildrenGet<MedicalBox>();
            _randomBox ??= ChildrenGet<RandomBox>();
        }

        public void OnActive(int currentLoot, int percentFreeChance)
        {
            if (GetRandom(percentFreeChance))
            {
                Debug.Log("Прок бесплатного лута");
            }
            else
            {
                Debug.Log("Прок платного лута");
            }
            
            switch (currentLoot)
            {
               case (int)TypeLoot.Money:
                   _randomBox.OnActive(currentLoot);
                   break;
               
               case (int)TypeLoot.Gem:
                   _randomBox.OnActive(currentLoot);
                   break;

               case (int)TypeLoot.Upgrade:
                   _randomBox.OnActive(currentLoot);
                   break;
               
               case (int)TypeLoot.Magnet:
                   _magnet.OnActive();
                   break;
               
               case (int)TypeLoot.Health:
                   _medicalBox.OnActive();
                   break;
               
               default:
                   _medicalBox.OnActive();
                   Debug.Log("incorrect choice loot spawn");
                   break;
            }
        }

        public void InActive() => 
            gameObject.SetActive(false);

        private bool GetRandom(int percentFreeChance) => 
            Random.Range(Single.Epsilon, MaxInclusive) <= percentFreeChance;
    }
}