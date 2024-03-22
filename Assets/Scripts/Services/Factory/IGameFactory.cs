using Ammo;
using Assistant;
using CameraModule;
using Canvases;
using Enemies;
using Infrastructure.Factory.Pools;
using Player;
using Spawners;
using Turrets;

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
        EnemySpawner CreateEnemySpawner();
        Turret CreateTurret();
        Missile CreateMissile();
        WindowRoot CreateWindowRoot();
    }
}