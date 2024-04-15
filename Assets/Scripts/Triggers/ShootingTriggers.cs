using Enemies;
using Player;
using Player.ShootingModule;
using Plugins.MonoCache;
using UnityEngine;

namespace Triggers
{
    public class ShootingTriggers : MonoCache
    {
        [HideInInspector] [SerializeField] private WeaponHolder _weaponHolder;
        [SerializeField] private HeroMovement _heroMovement;

        private Enemy _enemy;

        private void OnValidate() => 
            _weaponHolder ??= ChildrenGet<WeaponHolder>();

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                _enemy = enemy;
                
                _weaponHolder.GetActiveGun().Shoot(enemy);
                _heroMovement.SetStateBattle(true, enemy.transform);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy) == _enemy)
            {
                _weaponHolder.GetActiveGun().OffShoot();
                _heroMovement.SetStateBattle(false, null);
            }
        }
    }
}