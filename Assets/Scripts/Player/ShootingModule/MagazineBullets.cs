using System;
using Canvases;

namespace Player.ShootingModule
{
    public class MagazineBullets : IMagazine
    {
        
        private const int DelayRegenerationMagazine = 5000;
        
        private readonly HeroRegeneration _regeneration;
        private readonly int _maxSize;
        private int _size;
        private Hud _hud;

        public MagazineBullets(int size, Hud hud)
        {
            _hud = hud;
            hud.WeaponReload(false);
            _size = size;
            _maxSize = size;
            _regeneration = new HeroRegeneration(this);
        }

        public void Spend() => 
            _size--;

        public bool Check() => 
            _size != 0;

        public void Replenishment(Action fulled)
        {
            _hud.WeaponReload(true);
            _size++;

            if (_size >= _maxSize)
            {
                _hud.WeaponReload(false);
                fulled?.Invoke();
            }
        }

        public void Shortage()
        {
            if (_regeneration.IsWaiting)
                return;

            if (_size < _maxSize)
            {
                _regeneration.StopReplenishment();
                _regeneration.Launch(DelayRegenerationMagazine).Forget();
            }
        }
    }
}