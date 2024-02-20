using System.Collections.Generic;
using Turret;
using UnityEngine;

namespace Assistant
{
    [RequireComponent(typeof(MovementState))]
    public class IdleState : State
    {
        private MovementState _movementState;

        private List<CartridgeGun> _actualCartridgeGuns;
        private List<Vector3> _points = new();
        private Vector3 _storageAmmoPlate;
        private BasketAssistant _basket;

        private void OnValidate() =>
            _movementState = Get<MovementState>();

        public override void OnActive()
        {
            AnimatorCached.SetBool(AssistantData.IdleHash, true);
            
            if (_actualCartridgeGuns != null)
            {
                foreach (CartridgeGun cartridgeGun in _actualCartridgeGuns)
                {
                    cartridgeGun.DownloadRequired += AddPoints;
                    cartridgeGun.Fulled += RemovePoint;
                }
            }
            else
            {
                Debug.Log("Список актуальных точек пушек отсутствует");
            }

            if (_basket.IsEmpty)
            {
                _movementState.SetPoint(_storageAmmoPlate);
                StateMachine.EnterBehavior<MovementState>();
                
                return;
            }

            StandbyMode().Forget();
        }

        public override void InActive()
        {
            AnimatorCached.SetBool(AssistantData.IdleHash, false);
            
            if (_actualCartridgeGuns != null)
            {
                foreach (CartridgeGun cartridgeGun in _actualCartridgeGuns)
                {
                    cartridgeGun.DownloadRequired -= AddPoints;
                    cartridgeGun.Fulled -= RemovePoint;
                }
            }
            else
            {
                Debug.Log("Список актуальных точек пушек отсутствует");
            }
        }

        public void Inject(Vector3 storageAmmoPlate, BasketAssistant basketAssistant)
        {
            _basket = basketAssistant;
            _storageAmmoPlate = storageAmmoPlate;
        }

        public void AddPoints(Vector3 newPoint)
        {
            _points.Add(newPoint);
            _movementState.SetPoint(newPoint);
            StateMachine.EnterBehavior<MovementState>();
        }

        public void RemovePoint(Vector3 point) =>
            _points.Remove(point);
        
        public void SetActualPoint(List<CartridgeGun> actualCartridgeGuns) =>
            _actualCartridgeGuns = actualCartridgeGuns;
    }
}