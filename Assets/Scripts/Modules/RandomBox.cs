using Plugins.MonoCache;
using UnityEngine;

namespace Modules
{
    public class RandomBox : MonoCache
    {
        [Header("Money box meshes")]
        [SerializeField] private Mesh _meshUpMoney;
        [SerializeField] private Mesh _meshBottomMoney;
        [Header("Gem box meshes")]
        [SerializeField] private Mesh _meshUpGem;
        [SerializeField] private Mesh _meshBottomGem;
        [Header("Upgrade box meshes")]
        [SerializeField] private Mesh _meshUpUpgrade;
        [SerializeField] private Mesh _meshBottomUpgrade;

        public void OnActive(int currentLoot)
        {
            throw new System.NotImplementedException();
        }
    }
}