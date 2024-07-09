﻿using Canvases;
using ContactZones;
using Infrastructure.Factory.Pools;
using Player;
using SO;
using Spawners;
using Triggers;

namespace Modules
{
    public class LevelModule
    {
        private readonly FinishPlate _finishPlate;
        private readonly Pool _pool;
        private readonly Hero _hero;
        private readonly WorkerSpawner _workerSpawner;
        private readonly SectionPlate[] _sectionPlates;
        private readonly TransitionPlate[] _transitionPlates;
        private readonly BaseGate _baseGate;
        private readonly WindowModule _windowModule;
        private readonly PoolData _poolData;
        private readonly EnemySpawner _enemySpawner;

        public LevelModule(PoolData poolData, FinishPlate finishPlate, WindowModule windowModule, Pool pool, Hero hero,
            WorkerSpawner workerSpawner,
            SectionPlate[] sectionPlates, TransitionPlate[] transitionPlates, BaseGate baseGate,
            EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
            _poolData = poolData;
            _windowModule = windowModule;
            _baseGate = baseGate;
            _transitionPlates = transitionPlates;
            _sectionPlates = sectionPlates;
            _workerSpawner = workerSpawner;
            _hero = hero;
            _pool = pool;
            _finishPlate = finishPlate;
            
            _finishPlate.Finished += Up;
        }

        public void Dispose() => 
            _finishPlate.Finished -= Up;

        private void Up()
        {
            _poolData.CurrentLevelGame++;
            
            _windowModule.WinScreen();
            
            _pool.UpdateLevel();
            _enemySpawner.ActiveCurrentBoss();
            _hero.UpdateLevel();
            _baseGate.UpdateLevel();

            foreach (SectionPlate plate in _sectionPlates) 
                plate.UpdateLevel();

            foreach (TransitionPlate plate in _transitionPlates) 
                plate.UpdateLevel();
            
            _workerSpawner.UpdateLevel();

            _windowModule.UpLevelCompleted();
        }
    }
}