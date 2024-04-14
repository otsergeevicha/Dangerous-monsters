using Ammo;
using Assistant;
using Bank;
using CameraModule;
using Canvases;
using Enemies;
using Infrastructure.Factory.Pools;
using Loots;
using Player;
using Player.ShootingModule;
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
        Money CreateMoney();
        Bullet CreateBullet();
    }
}