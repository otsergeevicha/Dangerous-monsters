﻿using Agava.YandexGames;
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
using Services.SaveLoad;
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
        private TutorialModule _tutorialModule;
        private ISave _save;

        public void InstallBindings(ContainerBuilder descriptor) => 
            descriptor.OnContainerBuilt += LoadLevel;

        private void LoadLevel(Container container)
        {
            _save = container.Single<ISave>();
            
            ISDKService sdk = container.Single<ISDKService>();
            IInputService input = container.Single<IInputService>();
            IGameFactory gameFactory = container.Single<IGameFactory>();
            IWallet wallet = container.Single<IWallet>();
            
#if !UNITY_EDITOR
            CollationData();
#endif
            
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
            enemySpawner.Construct(_squareEnemySpawner, pool.PoolEnemies, _enemySpawnerData, pool.PoolBosses, _poolData, _firstLaunch);
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
                _tutorialModule = new TutorialModule(gameFactory, _storeTurretPlates, _storageAmmoPlate, 
                    _storeAssistantPlate, workerSpawner, 
                    _storageGem, _transitionPlates, _upgradePlayerBoard, cameraFollow,
                    _windowModule, enemySpawner, _turretPlates[0]);

                _firstLaunch = false;
            }
        }

        private void CollationData()
        {
            _firstLaunch = _save.AccessProgress().DataStateGame.FirstLaunch;

            _poolData.CurrentLevelGame = _save.AccessProgress().DataStateGame.CurrentLevel;

            _heroData.MaxHealth = _save.AccessProgress().DataStateGame.HeroHealth;
            _heroData.Speed = _save.AccessProgress().DataStateGame.HeroSpeed;
            _heroData.SizeBasket = _save.AccessProgress().DataStateGame.HeroSizeBasket;
            _heroData.RadiusDetection = _save.AccessProgress().DataStateGame.HeroRadiusDetection;

            _turretData.RadiusDetection = _save.AccessProgress().DataStateGame.RadiusDetection;
            _turretData.RotateSpeed = _save.AccessProgress().DataStateGame.RotateSpeed;

            _priceList.SectionPriceMultiplier = _save.AccessProgress().DataStateGame.SectionPriceMultiplier;
            _priceList.PriceTransitionPlate = _save.AccessProgress().DataStateGame.PriceTransitionPlate;
            _priceList.MultiplierIncreasePrice = _save.AccessProgress().DataStateGame.MultiplierIncreasePrice;
            _priceList.StartPriceTurret = _save.AccessProgress().DataStateGame.StartPriceTurret;
            _priceList.StepIncreasePriceTurret = _save.AccessProgress().DataStateGame.StepIncreasePriceTurret;
            _priceList.PriceHeroHealth = _save.AccessProgress().DataStateGame.PriceHeroHealth;
            _priceList.MultiplierPriceHeroHealth = _save.AccessProgress().DataStateGame.MultiplierPriceHeroHealth;
            _priceList.StepIncreaseHealthHero = _save.AccessProgress().DataStateGame.StepIncreaseHealthHero;
            _priceList.PriceHeroSpeed = _save.AccessProgress().DataStateGame.PriceHeroSpeed;
            _priceList.PriceHeroBasket = _save.AccessProgress().DataStateGame.PriceHeroBasket;
            _priceList.PriceHeroFiringRange = _save.AccessProgress().DataStateGame.PriceHeroFiringRange;
            _priceList.LoseBonusMoney = _save.AccessProgress().DataStateGame.LoseBonusMoney;
            _priceList.LoseBonusGem = _save.AccessProgress().DataStateGame.LoseBonusGem;
        }

        private void RecordData()
        {
            _save.AccessProgress().DataStateGame.FirstLaunch = _firstLaunch;

            _save.AccessProgress().DataStateGame.CurrentLevel = _poolData.CurrentLevelGame;

            _save.AccessProgress().DataStateGame.HeroHealth = _heroData.MaxHealth;
            _save.AccessProgress().DataStateGame.HeroSpeed = _heroData.Speed;
            _save.AccessProgress().DataStateGame.HeroSizeBasket = _heroData.SizeBasket;
            _save.AccessProgress().DataStateGame.HeroRadiusDetection = _heroData.RadiusDetection;

            _save.AccessProgress().DataStateGame.RadiusDetection = _turretData.RadiusDetection;
            _save.AccessProgress().DataStateGame.RotateSpeed = _turretData.RotateSpeed;

            _save.AccessProgress().DataStateGame.SectionPriceMultiplier = _priceList.SectionPriceMultiplier;
            _save.AccessProgress().DataStateGame.PriceTransitionPlate = _priceList.PriceTransitionPlate;
            _save.AccessProgress().DataStateGame.MultiplierIncreasePrice = _priceList.MultiplierIncreasePrice;
            _save.AccessProgress().DataStateGame.StartPriceTurret = _priceList.StartPriceTurret;
            _save.AccessProgress().DataStateGame.StepIncreasePriceTurret = _priceList.StepIncreasePriceTurret;
            _save.AccessProgress().DataStateGame.PriceHeroHealth = _priceList.PriceHeroHealth;
            _save.AccessProgress().DataStateGame.MultiplierPriceHeroHealth = _priceList.MultiplierPriceHeroHealth;
            _save.AccessProgress().DataStateGame.StepIncreaseHealthHero = _priceList.StepIncreaseHealthHero;
            _save.AccessProgress().DataStateGame.PriceHeroSpeed = _priceList.PriceHeroSpeed;
            _save.AccessProgress().DataStateGame.PriceHeroBasket = _priceList.PriceHeroBasket;
            _save.AccessProgress().DataStateGame.PriceHeroFiringRange = _priceList.PriceHeroFiringRange;
            _save.AccessProgress().DataStateGame.LoseBonusMoney = _priceList.LoseBonusMoney;
            _save.AccessProgress().DataStateGame.LoseBonusGem = _priceList.LoseBonusGem;
            
            _save.Save();
        }

        protected override void OnDisabled()
        {
#if !UNITY_EDITOR
            RecordData();
#endif
            _windowModule.Dispose();
            _levelModule.Dispose();
            _tutorialModule?.Dispose();
        }
    }
}