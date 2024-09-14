using System;
using Agava.YandexGames;
using Player;
using Plugins.MonoCache;
using UnityEngine;

namespace Loots
{
    public class RandomBox : MonoCache, ILoot
    {
        [SerializeField] private MeshFilter _upMeshFilter;
        [SerializeField] private MeshFilter _bottomMeshFilter;
        
        [Header("Money box meshes")]
        [SerializeField] private Mesh _meshUpMoney;
        [SerializeField] private Mesh _meshBottomMoney;
        [Header("Gem box meshes")]
        [SerializeField] private Mesh _meshUpGem;
        [SerializeField] private Mesh _meshBottomGem;

        [Header("Animators")] 
        [SerializeField] private Animator _animatorBody;
        [SerializeField] private Animator _animatorCap;

        [SerializeField] private GameObject _gem;
        [SerializeField] private GameObject _money;

        private const int AmountMoney = 100;
        private const int AmountGem = 100;

        private const string RuCoins = "монеты";
        private const string EngCoins = "coins";
        
        private const string RuGem = "кристалы";
        private const string EngGem = "crystals";

        private string _engName;
        private string _ruName;

        private Hero _hero;
        private int _currentLoot;

        public void Construct(Hero hero) => 
            _hero = hero;

        public void OnActive(int currentLoot)
        {
            _currentLoot = currentLoot;
            
            switch (_currentLoot)
            {
                case (int)TypeLoot.Money:
                {
                    _money.SetActive(true);
                    SetMeshFilter(_meshUpMoney, _meshBottomMoney);
                    
#if !UNITY_EDITOR
                    if (YandexGamesSdk.Environment.i18n.lang == "en")
                        _engName = EngCoins;
                    else
                        _ruName = RuCoins;
                    break;
#endif
                    _ruName = RuCoins;
                    
                    break;
                }
                case (int)TypeLoot.Gem:
                {
                    _gem.SetActive(true);
                    SetMeshFilter(_meshUpGem, _meshBottomGem);
                    
#if !UNITY_EDITOR
                    if (YandexGamesSdk.Environment.i18n.lang == "en")
                        _engName = EngGem;
                    else
                        _ruName = RuGem;
                    break;
#endif
                    _ruName = RuGem;
                    
                    break;
                }
                default:
                    _money.SetActive(true);
                    SetMeshFilter(_meshUpMoney, _meshBottomMoney);
                    
#if !UNITY_EDITOR
                    if (YandexGamesSdk.Environment.i18n.lang == "en")
                        _engName = EngCoins;
                    else
                        _ruName = RuCoins;
                    break;
#endif
                    _ruName = RuCoins;
                    break;
            }
            
            _animatorBody.enabled = true;
            
            gameObject.SetActive(true);
        }

        public void Open(Action opened)
        {
            switch (_currentLoot)
            {
                case (int)TypeLoot.Money:
                    _hero.ApplyMoney(AmountMoney);
                    break;
                case (int)TypeLoot.Gem:
                    _hero.ApplyGem(AmountGem);
                    break;
                default:
                    _hero.ApplyMoney(AmountMoney);
                    Debug.Log("Incorrect choice mesh filter");
                    break;
            }
            
            opened?.Invoke();
            _animatorBody.enabled = false;
        }

        public string GetName()
        {
#if !UNITY_EDITOR
            return YandexGamesSdk.Environment.i18n.lang == "en" 
                ? _engName 
                : _ruName;
#endif
            return _ruName;
        }

        public void InActive()
        {
            _animatorBody.enabled = true;
            
            _money.SetActive(false);
            _gem.SetActive(false);
            
            gameObject.SetActive(false);
        }

        private void SetMeshFilter(Mesh cap, Mesh body)
        {
            _upMeshFilter.mesh = cap;
            _bottomMeshFilter.mesh = body;
        }
    }
}