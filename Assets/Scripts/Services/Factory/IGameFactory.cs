﻿using Ammo;
using Assistant;
using CameraModule;
using Canvases;
using Enemies;
using HpBar;
using Infrastructure.Factory.Pools;
using Loots;
using Modules;
using Player;
using RingZone;
using Spawners;
using Turrets;
using Workers;

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
        Hud CreateHud();
        Money CreateMoney();
        Bullet CreateBullet();
        HeroAimRing CreateHeroAimRing();
        EnemyRing CreateEnemyRing();
        HealthBar CreateHealthBar();
        LoseScreen CreateLoseScreen();
        StartScreen CreateStartScreen();
        LootPoint CreateLootPoint();
        Worker CreateWorker();
        WorkerSpawner CreateWorkerSpawner();
    }
}