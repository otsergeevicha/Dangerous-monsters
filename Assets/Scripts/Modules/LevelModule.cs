using Canvases;
using ContactZones;
using Infrastructure.Factory.Pools;
using Player;
using Services.SaveLoad;
using SO;
using Spawners;
using Triggers;
using Turrets.Children;

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
        private readonly StoreTurretPlate[] _storeTurretPlates;
        private readonly CartridgeGun[] _cartridgeGuns;
        private ISave _save;

        public LevelModule(PoolData poolData, FinishPlate finishPlate, WindowModule windowModule, Pool pool, Hero hero,
            WorkerSpawner workerSpawner,
            SectionPlate[] sectionPlates, TransitionPlate[] transitionPlates, BaseGate baseGate,
            EnemySpawner enemySpawner, BaseView baseView, StoreTurretPlate[] storeTurretPlates,
            CartridgeGun[] cartridgeGuns, ISave save)
        {
            _save = save;
            _cartridgeGuns = cartridgeGuns;
            _storeTurretPlates = storeTurretPlates;
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
            ResetLevelData();

            _windowModule.WinScreen();
            _poolData.CurrentLevelGame++;
            _enemySpawner.ClearField();
            _pool.UpdateLevel();

            foreach (StoreTurretPlate plate in _storeTurretPlates) 
                plate.UpdateLevel();

            foreach (CartridgeGun cartridge in _cartridgeGuns) 
                cartridge.UpdateLevel();

            _baseGate.UpdateLevel();
            _hero.UpdateLevel();

            _enemySpawner.ActiveCurrentBoss();
            _enemySpawner.OnStart();

            foreach (SectionPlate plate in _sectionPlates) 
                plate.UpdateLevel();

            foreach (TransitionPlate plate in _transitionPlates) 
                plate.UpdateLevel();
            
            _workerSpawner.UpdateLevel();
            _windowModule.UpLevelCompleted();
            
            _baseView.UpdateText(_poolData.CurrentLevelGame.ToString());

            _finishPlate.InActive();
        }

        private void ResetLevelData()
        {
            _save.AccessProgress().DataStateLevel.HaveLeftTurret = false;
            _save.AccessProgress().DataStateLevel.HaveCenterTurret = false;
            _save.AccessProgress().DataStateLevel.HaveRightTurret = false;

            _save.AccessProgress().DataStateLevel.OpenLeftSection = false;
            _save.AccessProgress().DataStateLevel.OpenRightSection = false;

            _save.AccessProgress().DataStateLevel.OpenOneTransition = false;
            _save.AccessProgress().DataStateLevel.OpenTwoTransition = false;
            
            _save.Save();
        }
    }
}