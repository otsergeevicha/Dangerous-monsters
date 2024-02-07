using Ammo;
using CameraModule;
using Infrastructure.Factory.Pools;
using Player;
using Reflex;
using Services.Assets;
using Services.Factory;

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
    }
}