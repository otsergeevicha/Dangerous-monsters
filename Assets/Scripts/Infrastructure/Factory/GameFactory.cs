﻿using Ammo;
using Assistant;
using CameraModule;
using Enemies;
using Infrastructure.Factory.Pools;
using Player;
using Services.Assets;
using Services.Factory;
using Spawners;

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
    }
}