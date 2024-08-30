using System;
using Plugins.MonoCache;
using UnityEngine;

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

        public event Action<int> OnChanged;

        public void ChangeActive(bool flag) => 
            _canvas.enabled = flag;

        public void ActiveGun(int currentGun)
        {
            switch (currentGun)
            {
                case (int)WeaponType.Uzi:
                    OnChanged?.Invoke(currentGun);
                    break;
                case (int)WeaponType.AutoPistol:
                    OnChanged?.Invoke(currentGun);
                    break;
                case (int)WeaponType.MiniGun:
                    OnChanged?.Invoke(currentGun);
                    break;
                case (int)WeaponType.Rifle:
                    OnChanged?.Invoke(currentGun);
                    break;
                default:
                    OnChanged?.Invoke((int)WeaponType.Uzi);
                    break;
            }
        }
    }
}