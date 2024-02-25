using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ContactPlatforms;
using Plugins.MonoCache;
using SO;
using Turret;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
    [RequireComponent(typeof(BotInput))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class CargoAssistant : MonoCache
    {
        public StorageAmmoPlate StorageAmmoPlate { get; private set; }
        public CartridgeGun[] CartridgeGuns { get; private set; }

        //IsHaveCargo нужна логика проверки корзины на груз
        public bool IsHaveCargo { get; set; }

        public void Construct(AssistantData assistantData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate)
        {
            StorageAmmoPlate = storageAmmoPlate;
            CartridgeGuns = cartridgeGuns;
        }

        public void InActive()
        {
            gameObject.SetActive(false);
        }
    }

    public class SetDirection : Action
    {
        public SharedVector2 Direction;
        private CargoAssistant _cargoAssistant;

        public override void OnAwake() =>
            _cargoAssistant = GetComponent<CargoAssistant>();

        public override TaskStatus OnUpdate()
        {
            if (_cargoAssistant.IsHaveCargo)
            {
                for (int i = 0; i < _cargoAssistant.CartridgeGuns.Length; i++)
                {
                    if (_cargoAssistant.CartridgeGuns[i].IsRequiredDownload)
                    {
                        Direction.Value = _cargoAssistant.CartridgeGuns[i].transform.position;
                        return TaskStatus.Success;
                    }
                }

                Direction.Value = Vector2.zero;
                return TaskStatus.Success;
            }
            else
            {
                Direction.Value = _cargoAssistant.StorageAmmoPlate.transform.position;
                return TaskStatus.Success;
            }
        }
    }

    public class SetMovement : Action
    {
        public SharedBotInput SelfBotInput;
        public SharedVector2 Direction;

        private IBotInput _inputSource;
        private NavMeshAgent _agent;

        public override void OnAwake()
        {
            _inputSource = GetComponent<BotInput>();
            _agent = GetComponent<NavMeshAgent>();
        }

        public override void OnStart()
        {
            SelfBotInput.Value.MovementInput = Direction.Value;
            _agent.SetDestination(new Vector3(_inputSource.MovementInput.x, 0f, _inputSource.MovementInput.y));
        }
    }
}