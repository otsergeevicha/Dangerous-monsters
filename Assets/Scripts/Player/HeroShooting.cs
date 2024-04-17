using Player.Animation;
using Player.ShootingModule;
using Plugins.MonoCache;
using Triggers;

namespace Player
{
    public class HeroShooting : MonoCache
    {
        private ShootingTriggers _triggers;
        private HeroAnimation _heroAnimation;
        private WeaponHolder _weaponHolder;
        private HeroMovement _heroMovement;

        public void Construct(ShootingTriggers triggers, HeroAnimation heroAnimation, HeroMovement heroMovement,
            WeaponHolder weaponHolder)
        {
            _heroMovement = heroMovement;
            _weaponHolder = weaponHolder;
            _heroAnimation = heroAnimation;
            _triggers = triggers;
            
            _triggers.OnZone += Shoot;
        }

        protected override void OnDisabled() => 
            _triggers.OnZone -= Shoot;

        private void Shoot()
        {
            _heroAnimation.EnableShoot();
            _weaponHolder.GetActiveGun().Shoot(_triggers.CurrentEnemy);
            _heroMovement.SetStateBattle(true, _triggers.CurrentEnemy.transform);
        }
    }
}