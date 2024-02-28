using Ammo;
using Assistant;
using CameraModule;
using Infrastructure.Factory.Pools;
using Player;
using Reflex;

namespace Services.Factory
{
    public interface IGameFactory
    {
        Hero CreateHero();
        CameraFollow CreateCamera();
        AmmoBox CreateAmmoBox();
        Pool CreatePool();
        CargoAssistant CreateCargoAssistant();
        Enemy CreateEnemy(string currentPath);
    }
}