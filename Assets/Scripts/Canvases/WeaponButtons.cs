using System;
using Plugins.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace Canvases
{
    enum WeaponType
    {
        Uzi,
        AutoPistol,
        Rifle,
        MiniGun
    }

    public class WeaponButtons : MonoCache
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private Color[] _colors = new Color[2];
        [SerializeField] private Image[] _icons = new Image[4];

        public event Action<int> OnChanged;

        public void ChangeActive(bool flag)
        {
            _canvas.enabled = flag;

            if (flag) 
                ActiveGun((int)WeaponType.Uzi);
        }

        public void ActiveGun(int currentGun)
        {
            foreach (Image icon in _icons) 
                icon.color = _colors[0];

            switch (currentGun)
            {
                case (int)WeaponType.Uzi:
                    Notify(currentGun);
                    break;
                case (int)WeaponType.AutoPistol:
                    Notify(currentGun);
                    break;
                case (int)WeaponType.MiniGun:
                    Notify(currentGun);
                    break;
                case (int)WeaponType.Rifle:
                    Notify(currentGun);
                    break;
                default:
                    Notify(currentGun);
                    break;
            }
        }

        private void Notify(int currentGun)
        {
            _icons[currentGun].color = _colors[1];
            OnChanged?.Invoke(currentGun);
        }
    }
}