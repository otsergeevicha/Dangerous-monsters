using Agava.YandexGames;
using CameraModule;
using Canvases;
using ContactPlatforms;
using Infrastructure.Factory.Pools;
using Player;
using Plugins.MonoCache;
using Reflex.Core;
using RingZone;
using Services.Bank;
using Services.Factory;
using Services.Inputs;
using Services.SDK;
using SO;
using Spawners;
using Triggers;
using Turrets;
using Turrets.Children;
using UnityEngine;

namespace Reflex
{
    public class MainSceneInstaller : MonoCache, IInstaller
    {
        [Header("Objects with scene")]
        [SerializeField] private CartridgeGun[] _cartridgeGuns;
        [SerializeField] private StorageAmmoPlate _storageAmmoPlate;
        [SerializeField] private Transform[] _squareEnemySpawner = new Transform[4];
        [SerializeField] private TurretPlate[] _turretPlates;
        [SerializeField] private StoreAssistantPlate _storeAssistantPlate;
        [SerializeField] private StoreTurretPlate[] _storeTurretPlates;
        [SerializeField] private SectionPlate[] _sectionPlates;
        [SerializeField] private BaseGate _baseGate;
        
        [Header("Required configurations")]
        [SerializeField] private HeroData _heroData;
        [SerializeField] private AssistantData _assistantData;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private PoolData _poolData;
        [SerializeField] private EnemySpawnerData _enemySpawnerData;
        [SerializeField] private BulletData _bulletData;
        [SerializeField] private TurretData _turretData;
        [SerializeField] private PriceListData _priceList;
        
        public void InstallBindings(ContainerBuilder descriptor) => 
            descriptor.OnContainerBuilt += LoadLevel;

        private void LoadLevel(Container container)
        {
            ISDKService sdk = container.Single<ISDKService>();
            IInputService input = container.Single<IInputService>();
            IGameFactory gameFactory = container.Single<IGameFactory>();
            IWallet wallet = container.Single<IWallet>();

            Pool pool = gameFactory.CreatePool();
            CameraFollow cameraFollow = gameFactory.CreateCamera();
            WindowRoot windowRoot = gameFactory.CreateWindowRoot();
            Hero hero = gameFactory.CreateHero();
            EnemySpawner enemySpawner = gameFactory.CreateEnemySpawner();
            HeroAimRing heroAimRing = gameFactory.CreateHeroAimRing();
            EnemyRing enemyRing = gameFactory.CreateEnemyRing();

            pool.Construct(gameFactory, _poolData, _assistantData, _enemyData, _cartridgeGuns, _storageAmmoPlate, _turretPlates, _bulletData, _turretData);
            hero.Construct(input, wallet, _heroData, pool.PoolAmmoBox, pool.PoolBullet, _poolData.MaxCountBullets, enemyRing, pool.PoolEnemies.Enemies, gameFactory.CreateHealthBar());
            cameraFollow.Construct(hero.GetCameraRoot());
            enemySpawner.Construct(_squareEnemySpawner, pool.PoolEnemies, _enemySpawnerData);

            foreach (SectionPlate sectionPlate in _sectionPlates) 
                sectionPlate.Construct(wallet, _priceList, _poolData);

            windowRoot.Construct(input, _storeAssistantPlate, _storeTurretPlates, _poolData, pool);
            heroAimRing.Construct(hero.transform, _heroData.RadiusDetection);
            _baseGate.Construct(heroAimRing, cameraFollow);

#if !UNITY_EDITOR
            YandexGamesSdk.GameReady();
#endif
        }
    }
}