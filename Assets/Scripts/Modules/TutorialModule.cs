using CameraModule;
using Canvases;
using Canvases.UpgradePlayer;
using ContactZones;
using Cysharp.Threading.Tasks;
using Markers;
using Services.Factory;
using Spawners;
using Turrets;

namespace Modules
{
    public class TutorialModule
    {
        private readonly TutorialMarker _tutorialMarker;
        private readonly StoreTurretPlate[] _storeTurretPlates;
        private readonly StorageAmmoPlate _storageAmmoPlate;
        private readonly StoreAssistantPlate _storeAssistantPlate;
        private readonly WorkerSpawner _workerSpawner;
        private readonly StorageGem _storageGem;
        private readonly TransitionPlate[] _transitionPlates;
        private readonly UpgradePlayerBoard _upgradePlayerBoard;
        private readonly CameraFollow _cameraFollow;
        private readonly WindowModule _windowModule;
        private readonly EnemySpawner _enemySpawner;
        private readonly TurretPlate _turretPlate;

        public TutorialModule(IGameFactory factory,
            StoreTurretPlate[] storeTurretPlates, StorageAmmoPlate storageAmmoPlate,
            StoreAssistantPlate storeAssistantPlate, WorkerSpawner workerSpawner,
            StorageGem storageGem, TransitionPlate[] transitionPlates, UpgradePlayerBoard upgradePlayerBoard,
            CameraFollow cameraFollow, WindowModule windowModule, EnemySpawner enemySpawner, TurretPlate turretPlate)
        {
            _turretPlate = turretPlate;
            _enemySpawner = enemySpawner;
            _windowModule = windowModule;
            _cameraFollow = cameraFollow;
            _upgradePlayerBoard = upgradePlayerBoard;
            _transitionPlates = transitionPlates;
            _storageGem = storageGem;
            _workerSpawner = workerSpawner;
            _storeAssistantPlate = storeAssistantPlate;
            _storageAmmoPlate = storageAmmoPlate;
            _storeTurretPlates = storeTurretPlates;

            _tutorialMarker = factory.CreateTutorialMarker();
            
            _windowModule.OnStartGame += OnStart;
            _storeAssistantPlate.OnTutorialContacted += WakeUpEnemies;
        }

        public void Dispose()
        {
            _windowModule.OnStartGame -= OnStart;
            _storeAssistantPlate.OnTutorialContacted -= WakeUpEnemies;
        }

        private void OnStart() =>
            Launch().Forget();

        private void WakeUpEnemies() => 
            _enemySpawner.OnStart();

        private async UniTaskVoid Launch()
        {
            //первым шагов сделать показ требование победы над боссом
            await TutorialStep(_storeTurretPlates[0]);
            await TutorialStep(_storageAmmoPlate);
            await TutorialStep(_turretPlate.GetCartridgeGun);
            await TutorialStep(_storeAssistantPlate);
            await TutorialStep(_workerSpawner);
            await TutorialStep(_storageGem);
            await TutorialStep(_transitionPlates[0]);
            await TutorialStep(_upgradePlayerBoard);
        }
        
        private async UniTask TutorialStep(ITutorialPlate tutorialPlate)
        {
            UniTaskCompletionSource token = new UniTaskCompletionSource();

            _tutorialMarker.OnActive(tutorialPlate.GetPositionMarker());
            _cameraFollow.ShowMarker(tutorialPlate.GetRootCamera());

            tutorialPlate.OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            tutorialPlate.OnTutorialContacted -= () =>
                token.TrySetResult();
        }
    }
}