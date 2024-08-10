using Agava.YandexGames;
using CameraModule;
using Canvases;
using Canvases.UpgradePlayer;
using ContactZones;
using Infrastructure.Factory.Pools;
using Infrastructure.SDK;
using Markers;
using Modules;
using Player;
using Plugins.MonoCache;
using Reflex.Core;
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
    [RequireComponent(typeof(FocusGame))]
    public class MainSceneInstaller : MonoCache, IInstaller
    {
        [SerializeField] private bool _firstLaunch;
        
        [Header("Objects with scene")] 
        [SerializeField] private FocusGame _focusGame;
        [SerializeField] private UpgradePlayerBoard _upgradePlayerBoard;
        [SerializeField] private FinishPlate _finishPlate;
        [SerializeField] private Transform _spawnPointBoss;
        [SerializeField] private Workplace _workplace;
        [SerializeField] private StorageGem _storageGem;
        [SerializeField] private Transform[] _gemMiners = new Transform[3];
        [SerializeField] private TransitionPlate[] _transitionPlates = new TransitionPlate[2];
        [SerializeField] private CartridgeGun[] _cartridgeGuns;
        [SerializeField] private StorageAmmoPlate _storageAmmoPlate;
        [SerializeField] private Transform[] _squareEnemySpawner = new Transform[4];
        [SerializeField] private Transform[] _squareLootSpawner = new Transform[4];
        [SerializeField] private TurretPlate[] _turretPlates;
        [SerializeField] private StoreAssistantPlate _storeAssistantPlate;
        [SerializeField] private StoreTurretPlate[] _storeTurretPlates;
        [SerializeField] private SectionPlate[] _sectionPlates;
        [SerializeField] private BaseGate _baseGate;
        [SerializeField] private MonstersPortal _monstersPortal;
        
        [Header("Required configurations")]
        [SerializeField] private HeroData _heroData;
        [SerializeField] private AssistantData _assistantData;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private PoolData _poolData;
        [SerializeField] private EnemySpawnerData _enemySpawnerData;
        [SerializeField] private BulletData _bulletData;
        [SerializeField] private TurretData _turretData;
        [SerializeField] private PriceListData _priceList;
        [SerializeField] private WorkerData _workerData;

        private WindowModule _windowModule;
        private LevelModule _levelModule;
        private EffectModule _effectModule;

        public void InstallBindings(ContainerBuilder descriptor) => 
            descriptor.OnContainerBuilt += LoadLevel;

        private void LoadLevel(Container container)
        {
            ISDKService sdk = container.Single<ISDKService>();
            IInputService input = container.Single<IInputService>();
            IGameFactory gameFactory = container.Single<IGameFactory>();
            IWallet wallet = container.Single<IWallet>();

            _windowModule = new WindowModule();
            _effectModule = new EffectModule();
            
            Hud hud = gameFactory.CreateHud();
            WinScreen winScreen = gameFactory.CreateWinScreen();
            LoseScreen loseScreen = gameFactory.CreateLoseScreen();
            StartScreen startScreen = gameFactory.CreateStartScreen();
            UpgradeHeroScreen upgradeHeroScreen = gameFactory.CreateUpgradeHeroScreen();
            Pool pool = gameFactory.CreatePool();
            CameraFollow cameraFollow = gameFactory.CreateCamera();
            Hero hero = gameFactory.CreateHero();
            EnemySpawner enemySpawner = gameFactory.CreateEnemySpawner();
            WorkerSpawner workerSpawner = gameFactory.CreateWorkerSpawner();
            HeroAimRing heroAimRing = gameFactory.CreateHeroAimRing();
            EnemyRing enemyRing = gameFactory.CreateEnemyRing();

            sdk.Inject(cameraFollow.GetListener);
            
#if !UNITY_EDITOR
            _focusGame.Construct(cameraFollow.GetListener);
#endif
            
            pool.Construct(gameFactory, _poolData, _assistantData, _enemyData, _cartridgeGuns, _storageAmmoPlate, 
                _turretPlates, _bulletData, _turretData, _squareLootSpawner, sdk, _workerData, _gemMiners, _storageGem, 
                _spawnPointBoss.position, _finishPlate, hero, _monstersPortal.transform.position, _effectModule);

            _effectModule.Construct(pool.PoolEffects);
            
            heroAimRing.Construct(hero.transform, _heroData.RadiusDetection);
            
            hero.Construct(input, wallet, _heroData, pool.PoolAmmoBox, pool.PoolBullet, _poolData.MaxCountBullets, 
                enemyRing, pool.PoolEnemies.Enemies, pool.PoolBosses.Bosses, hud, _windowModule, cameraFollow, heroAimRing);
            
            cameraFollow.Construct(hero.GetCameraRoot());
            enemySpawner.Construct(_squareEnemySpawner, pool.PoolEnemies, _enemySpawnerData, pool.PoolBosses, _poolData);
            workerSpawner.Construct(pool.PoolWorkers, _workplace.gameObject.transform.position);
            _workplace.Construct(_poolData.MaxCountWorkers);

            foreach (SectionPlate sectionPlate in _sectionPlates) 
                sectionPlate.Construct(wallet, _priceList, _poolData, sdk);

            foreach (TransitionPlate plate in _transitionPlates)
                plate.Construct(wallet, _priceList);

            hud.Construct(cameraFollow.GetCameraMain, _monstersPortal, input);

            _windowModule.Construct(_storeAssistantPlate, _storeTurretPlates, _poolData, 
                pool, wallet, hud, loseScreen, startScreen, winScreen, input, _upgradePlayerBoard, upgradeHeroScreen, _heroData, _priceList, hero, sdk);
            
            _baseGate.Construct(heroAimRing, cameraFollow, hero);
            
            _levelModule = new LevelModule(_poolData, _finishPlate, _windowModule,  pool, hero, workerSpawner, _sectionPlates, _transitionPlates, _baseGate, enemySpawner);

            _finishPlate.InActive();
            
#if !UNITY_EDITOR
            YandexGamesSdk.GameReady();
#endif

            if (_firstLaunch)
            {
                TutorialModule tutorialModule = new TutorialModule(gameFactory, pool.PoolEnemies.Enemies, pool.PoolBosses.Bosses, 
                    _storeTurretPlates, _storageAmmoPlate, 
                    _storeAssistantPlate, workerSpawner, 
                    _storageGem, _transitionPlates, _upgradePlayerBoard, cameraFollow);
            }
        }

        protected override void OnDisabled()
        {
            _windowModule.Dispose();
            _levelModule.Dispose();
        }
    }
}