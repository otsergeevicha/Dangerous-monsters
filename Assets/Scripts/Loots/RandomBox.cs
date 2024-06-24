using System;
using Modules;
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
        [Header("Upgrade box meshes")]
        [SerializeField] private Mesh _meshUpUpgrade;
        [SerializeField] private Mesh _meshBottomUpgrade;

        [Header("Animators")] 
        [SerializeField] private Animator _animatorBody;
        [SerializeField] private Animator _animatorCap;

        public void OnActive(int currentLoot)
        {
            switch (currentLoot)
            {
                case (int)TypeLoot.Money:
                    SetMeshFilter(_meshUpMoney, _meshBottomMoney);
                    break;
                case (int)TypeLoot.Gem:
                    SetMeshFilter(_meshUpGem, _meshBottomGem);
                    break;
                case (int)TypeLoot.Upgrade:
                    SetMeshFilter(_meshUpUpgrade, _meshBottomUpgrade);
                    break;
                default:
                    SetMeshFilter(_meshUpUpgrade, _meshBottomUpgrade);
                    Debug.Log("Incorrect choice mesh filter");
                    break;
            }
            
            _animatorBody.enabled = false;
            _animatorCap.enabled = true;
            
            gameObject.SetActive(true);
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

        private void SetMeshFilter(Mesh cap, Mesh body)
        {
            _upMeshFilter.mesh = cap;
            _bottomMeshFilter.mesh = body;
        }
    }
}