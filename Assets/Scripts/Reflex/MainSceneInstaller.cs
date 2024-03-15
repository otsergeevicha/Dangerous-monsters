using CameraModule;
using ContactPlatforms;
using Infrastructure.Factory.Pools;
using Player;
using Plugins.MonoCache;
using Reflex.Core;
using Services.Factory;
using Services.Inputs;
using SO;
using Spawners;
using Turrets;
using Turrets.Childrens;
using UnityEngine;

namespace Reflex
{
    public class MainSceneInstaller : MonoCache, IInstaller
    {
        [SerializeField] private CartridgeGun[] _cartridgeGuns;
        [SerializeField] private StorageAmmoPlate _storageAmmoPlate;
        [SerializeField] private Transform[] _squareEnemySpawner = new Transform[4];
        [SerializeField] private TurretPlate[] _turretPlates;
        
        [SerializeField] private HeroData _heroData;
        [SerializeField] private AssistantData _assistantData;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private PoolData _poolData;
        [SerializeField] private EnemySpawnerData _enemySpawnerData;
        [SerializeField] private AmmoData _ammoData;
        
        public void InstallBindings(ContainerBuilder descriptor) => 
            descriptor.OnContainerBuilt += LoadLevel;

        private void LoadLevel(Container container)
        {
            IInputService input = container.Single<IInputService>();
            IGameFactory gameFactory = container.Single<IGameFactory>();

            Pool pool = gameFactory.CreatePool();
            CameraFollow cameraFollow = gameFactory.CreateCamera();
            Hero hero = gameFactory.CreateHero();
            EnemySpawner enemySpawner = gameFactory.CreateEnemySpawner();

            pool.Construct(gameFactory, _poolData, _assistantData, _enemyData, _cartridgeGuns, _storageAmmoPlate, _turretPlates, _ammoData);
            hero.Construct(input, _heroData, pool.PoolAmmoBox);
            cameraFollow.Construct(hero.GetCameraRoot());
            enemySpawner.Construct(_squareEnemySpawner, pool.PoolEnemies, _enemySpawnerData);
        }
    }
}