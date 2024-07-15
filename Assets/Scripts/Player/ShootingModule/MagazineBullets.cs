using System;
using Canvases;

namespace Player.ShootingModule
{
    public class MagazineBullets : IMagazine
    {
        private const int DelayRegenerationMagazine = 5000;

        private readonly MagazineReload _magazineReload;
        private readonly int _maxSize;
        private readonly Hud _hud;
        private int _size;

        public MagazineBullets(int size, Hud hud)
        {
            _hud = hud;
            hud.WeaponReload(false);
            _size = size;
            _maxSize = size;
            _magazineReload = new MagazineReload(this);
        }

        public void Spend() =>
            _size--;

        public bool Check()
        {
            if (_size == 0)
            {
                _hud.WeaponReload(true);
                return false;
            }

            _hud.WeaponReload(false);
            return true;
        }

        public void Replenishment(Action fulled)
        {
            _size++;

            if (_size >= _maxSize)
            {
                _magazineReload.StopReplenishment();
                fulled?.Invoke();
            }
        }

        public void Shortage()
        {
            if (_magazineReload.IsCharge)
                return;

            if (_size == 0)
            {
                _magazineReload.StopReplenishment();
                _magazineReload.Launch(DelayRegenerationMagazine).Forget();
            }
        }

        public void UpdateLevel() =>
            _size = _maxSize;
    }
}