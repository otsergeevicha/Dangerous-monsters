using System;
using HpBar;
using SO;
using UnityEngine;

namespace Modules
{
    public class HeroHealthModule
    {
        private int _maxHealth;
        private int _minHealth = 0;
        private int _currentHealth;
        private HealthBar _healthBar;
        private HeroData _heroData;

        public HeroHealthModule(HealthBar healthBar, HeroData heroData)
        {
            _healthBar = healthBar;
            _heroData = heroData;
            
            Reset();
        }

        public event Action Died;

        public void ApplyDamage(int damage)
        {
            _currentHealth -= Mathf.Clamp(damage, _minHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Died?.Invoke();
            }
            
            _healthBar.ChangeValue(_currentHealth, _maxHealth, damage);
        }

        public void Reset()
        {
            _healthBar.HealingValue();
            _maxHealth = _heroData.MaxHealth;
            _currentHealth = _heroData.MaxHealth;
        }
    }
}