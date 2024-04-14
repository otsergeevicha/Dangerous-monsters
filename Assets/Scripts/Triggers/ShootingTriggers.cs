using Enemies;
using Player.ShootingModule;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class ShootingTriggers : MonoCache
    {
        [SerializeField] private WeaponHolder _weaponHolder;

        private Enemy _enemy;

        private void OnValidate() => 
            _weaponHolder ??= ChildrenGet<WeaponHolder>();

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                _enemy = enemy;
                _weaponHolder.GetActiveGun().Shoot(enemy);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy) == _enemy)
                _weaponHolder.GetActiveGun().OffShoot();
        }
    }
}