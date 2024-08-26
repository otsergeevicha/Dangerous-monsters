using Canvases;
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
        private readonly BaseView _baseView;

        public LevelModule(PoolData poolData, FinishPlate finishPlate, WindowModule windowModule, Pool pool, Hero hero,
            WorkerSpawner workerSpawner,
            SectionPlate[] sectionPlates, TransitionPlate[] transitionPlates, BaseGate baseGate,
            EnemySpawner enemySpawner, BaseView baseView)
        {
            _baseView = baseView;
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
            _windowModule.WinScreen();
            _poolData.CurrentLevelGame++;

            _pool.UpdateLevel();
            _baseGate.UpdateLevel();
            _hero.UpdateLevel();
            _enemySpawner.ActiveCurrentBoss();

            foreach (SectionPlate plate in _sectionPlates) 
                plate.UpdateLevel();

            foreach (TransitionPlate plate in _transitionPlates) 
                plate.UpdateLevel();
            
            _workerSpawner.UpdateLevel();
            _windowModule.UpLevelCompleted();
            
            _baseView.UpdateText(_poolData.CurrentLevelGame.ToString());
        }
    }
}