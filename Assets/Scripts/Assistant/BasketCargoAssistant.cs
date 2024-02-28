using System.Linq;
using Ammo;
using Cysharp.Threading.Tasks;
using Plugins.MonoCache;
using Turret;
using UnityEngine;

namespace Assistant
{
    public class BasketCargoAssistant : MonoCache, IBasket
    {
        [SerializeField] private CargoAssistant _cargoAssistant;
        [SerializeField] private AmmoBox[] _ammoBoxes;
        
        private const int MillisecondsDelay = 500;
        
        private AmmoBox _box;
        private bool _isReplenishment;
        private int _maxSizeBasket;
        private int _currentCount = 0;

        public bool IsFull { get; private set; }
        public bool IsEmpty => 
            _currentCount == 0;

        private void Start()
        {
            _maxSizeBasket = _cargoAssistant.GetMaxSizeBasket;

            if (_ammoBoxes.Length != 0)
            {
                foreach (AmmoBox box in _ammoBoxes) 
                    box.InActive();
            }
        }


        public void StopReplenishment() => 
            _isReplenishment = false;

        public void SpendBox()
        {
            _box = _ammoBoxes.LastOrDefault(box =>
                box.isActiveAndEnabled);

            if (_box != null)
                _box.InActive();
            
            _box = null;
            _currentCount--;

            IsFull = false;
            
            if (_currentCount <= 0) 
                _currentCount = 0;
        }

        private bool CheckFull() => 
            _currentCount == _maxSizeBasket;

        public async UniTaskVoid Replenishment()
        {
            _isReplenishment = true;

            while (_isReplenishment)
            {
                if (CheckFull())
                {
                    IsFull = true;
                    _isReplenishment = false;
                }
                else
                {
                    _box = _ammoBoxes.FirstOrDefault(box =>
                        box.isActiveAndEnabled == false);

                    if (_box != null)
                        _box.OnActive();
            
                    _box = null;
                    
                    _currentCount++;
                }

                await UniTask.Delay(MillisecondsDelay);
            }
        }
    }
}