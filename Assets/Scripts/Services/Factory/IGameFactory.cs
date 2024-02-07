using CameraModule;
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
    }
}