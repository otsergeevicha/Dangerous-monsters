using Ammo;
using Assistant;
using Bank;
using CameraModule;
using Canvases;
using Enemies;
using HpBar;
using Infrastructure.Factory.Pools;
using Loots;
using Player;
using RingZone;
using Services.Assets;
using Services.Factory;
using Spawners;
using Turrets;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetsProvider _assetsProvider;

        public GameFactory(IAssetsProvider assetsProvider) =>
            _assetsProvider = assetsProvider;
        
        public Hero CreateHero() =>
            _assetsProvider.InstantiateEntity(Constants.HeroPath)
                .GetComponent<Hero>();
        
        public CameraFollow CreateCamera() =>
            _assetsProvider.InstantiateEntity(Constants.CameraPath)
                .GetComponent<CameraFollow>();

        public AmmoBox CreateAmmoBox() =>
            _assetsProvider.InstantiateEntity(Constants.AmmoBoxPath)
                .GetComponent<AmmoBox>();
        
        public Pool CreatePool() =>
            _assetsProvider.InstantiateEntity(Constants.PoolPath)
                .GetComponent<Pool>();

        public CargoAssistant CreateCargoAssistant() =>
            _assetsProvider.InstantiateEntity(Constants.CargoAssistantPath)
                .GetComponent<CargoAssistant>();

        public Enemy CreateEnemy(string currentPath) =>
            _assetsProvider.InstantiateEntity(currentPath)
                .GetComponent<Enemy>();

        public EnemySpawner CreateEnemySpawner() =>
            _assetsProvider.InstantiateEntity(Constants.EnemySpawnerPath)
                .GetComponent<EnemySpawner>();

        public Turret CreateTurret() =>
            _assetsProvider.InstantiateEntity(Constants.TurretPath)
                .GetComponent<Turret>();

        public Missile CreateMissile() =>
            _assetsProvider.InstantiateEntity(Constants.MissilePath)
                .GetComponent<Missile>();

        public WindowRoot CreateWindowRoot() =>
            _assetsProvider.InstantiateEntity(Constants.WindowRootPath)
                .GetComponent<WindowRoot>();

        public Money CreateMoney() =>
            _assetsProvider.InstantiateEntity(Constants.MoneyPath)
                .GetComponent<Money>();

        public Bullet CreateBullet() =>
            _assetsProvider.InstantiateEntity(Constants.BulletPath)
                .GetComponent<Bullet>();

        public HeroAimRing CreateHeroAimRing() =>
            _assetsProvider.InstantiateEntity(Constants.HeroAimRingPath)
                .GetComponent<HeroAimRing>();

        public EnemyRing CreateEnemyRing() =>
            _assetsProvider.InstantiateEntity(Constants.EnemyRingPath)
                .GetComponent<EnemyRing>();

        public HealthBar CreateHealthBar() =>
            _assetsProvider.InstantiateEntity(Constants.HPBarPath)
                .GetComponent<HealthBar>();
    }
}