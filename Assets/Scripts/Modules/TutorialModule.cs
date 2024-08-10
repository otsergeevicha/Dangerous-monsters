using System.Collections.Generic;
using CameraModule;
using Canvases;
using Canvases.UpgradePlayer;
using ContactZones;
using Cysharp.Threading.Tasks;
using Enemies;
using Markers;
using Services.Factory;
using Spawners;

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
        private readonly List<Enemy> _enemies;
        private readonly List<Enemy> _bosses;

        public TutorialModule(IGameFactory factory, List<Enemy> enemies, List<Enemy> bosses,
            StoreTurretPlate[] storeTurretPlates, StorageAmmoPlate storageAmmoPlate,
            StoreAssistantPlate storeAssistantPlate, WorkerSpawner workerSpawner,
            StorageGem storageGem, TransitionPlate[] transitionPlates, UpgradePlayerBoard upgradePlayerBoard,
            CameraFollow cameraFollow)
        {
            _bosses = bosses;
            _enemies = enemies;
            _cameraFollow = cameraFollow;
            _upgradePlayerBoard = upgradePlayerBoard;
            _transitionPlates = transitionPlates;
            _storageGem = storageGem;
            _workerSpawner = workerSpawner;
            _storeAssistantPlate = storeAssistantPlate;
            _storageAmmoPlate = storageAmmoPlate;
            _storeTurretPlates = storeTurretPlates;

            _tutorialMarker = factory.CreateTutorialMarker();

            Launch().Forget();
        }

        private async UniTaskVoid Launch()
        {
            UniTaskCompletionSource token = new UniTaskCompletionSource();
            
            foreach (Enemy enemy in _enemies)
                enemy.OnSleep();

            foreach (Enemy boss in _bosses)
                boss.OnSleep();
            
            _tutorialMarker.OnActive(_storeTurretPlates[0].GetPositionMarker());
            _cameraFollow.ShowMarker(_storeTurretPlates[0].GetRootCamera());

            _storeTurretPlates[0].OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            _storeTurretPlates[0].OnTutorialContacted -= () =>
                token.TrySetResult();
            
            _tutorialMarker.OnActive(_storageAmmoPlate.GetPositionMarker());
            _cameraFollow.ShowMarker(_storageAmmoPlate.GetRootCamera());
            
            _storageAmmoPlate.OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            _storageAmmoPlate.OnTutorialContacted -= () =>
                token.TrySetResult();
            
            _tutorialMarker.OnActive(_storeAssistantPlate.GetPositionMarker());
            _cameraFollow.ShowMarker(_storeAssistantPlate.GetRootCamera());
            
            _storeAssistantPlate.OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            _storeAssistantPlate.OnTutorialContacted -= () =>
                token.TrySetResult();
            
            foreach (Enemy enemy in _enemies)
                enemy.UnSleep();

            foreach (Enemy boss in _bosses)
                boss.UnSleep();
            
            _tutorialMarker.OnActive(_workerSpawner.GetPositionMarker());
            _cameraFollow.ShowMarker(_workerSpawner.GetRootCamera());
            
            _workerSpawner.OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            _workerSpawner.OnTutorialContacted -= () =>
                token.TrySetResult();
            
            _tutorialMarker.OnActive(_storageGem.GetPositionMarker());
            _cameraFollow.ShowMarker(_storageGem.GetRootCamera());
            
            _storageGem.OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            _storageGem.OnTutorialContacted -= () =>
                token.TrySetResult();
            
            _tutorialMarker.OnActive(_transitionPlates[0].GetPositionMarker());
            _cameraFollow.ShowMarker(_transitionPlates[0].GetRootCamera());
            
            _transitionPlates[0].OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            _transitionPlates[0].OnTutorialContacted -= () =>
                token.TrySetResult();

            _tutorialMarker.OnActive(_upgradePlayerBoard.GetPositionMarker());
            _cameraFollow.ShowMarker(_upgradePlayerBoard.GetRootCamera());
            
            _upgradePlayerBoard.OnTutorialContacted += () =>
                token.TrySetResult();
            await token.Task;
            _upgradePlayerBoard.OnTutorialContacted -= () =>
                token.TrySetResult();
        }
    }
}