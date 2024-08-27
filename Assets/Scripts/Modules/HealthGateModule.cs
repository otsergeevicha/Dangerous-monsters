using System;
using UnityEngine;

namespace Modules
{
    public class HealthGateModule
    {
        private const int Damage = 1;
        
        private readonly int _maxHealth;
        private readonly int _minHealth;
        
        private int _currentHealth;

        public HealthGateModule(int maxHealthBaseGate)
        {
            _maxHealth = maxHealthBaseGate;
            _currentHealth = _maxHealth;
        }

        public event Action OnGateFall;
        
        public void ApplyDamage()
        {
            _currentHealth -= Mathf.Clamp(Damage, _minHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnGateFall?.Invoke();
            }
        }

        public void Reset() => 
            _currentHealth = _maxHealth;
            
    }
}