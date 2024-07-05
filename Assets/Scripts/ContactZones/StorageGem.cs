using System.Linq;
using Loots;
using Plugins.MonoCache;
using UnityEngine;

namespace ContactZones
{
    public class StorageGem : MonoCache
    {
        [SerializeField] private Gem[] _gems = new Gem[27];

        private int _currentAmountGems = 0;
        private int _maxAmount;

        public bool IsFulled { get; private set; }

        private void Start()
        {
            _maxAmount = _gems.Length;

            if (_maxAmount != 0)
            {
                for (int i = 0; i < _maxAmount; i++)
                    _gems[i].InActive();
            }
        }

        public void ApplyGem()
        {
            if (_currentAmountGems >= _maxAmount)
            {
                _currentAmountGems = _maxAmount;
                IsFulled = true;
            }
            
            for (int i = 0; i < _maxAmount; i++)
            {
                if (_gems[i].isActiveAndEnabled == false)
                {
                    _gems[i].OnActive();
                    _currentAmountGems++;
                    return;
                }
            }
        }

        public void Spend()
        {
            Gem gem = _gems.LastOrDefault(gem => gem.isActiveAndEnabled);
            
            if (gem != null)
            {
                _currentAmountGems--;
                IsFulled = false;
                
                if (_currentAmountGems < 0) 
                    _currentAmountGems = 0;
                
                gem.InActive();
            }
        }
    }
}