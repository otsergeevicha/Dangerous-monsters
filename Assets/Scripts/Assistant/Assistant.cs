using System;
using System.Collections.Generic;
using ContactPlatforms;
using Plugins.MonoCache;
using SO;
using Turret;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
    enum TypeAssistant
    {
        MoneyAssistant,
        CargoAssistant
    }

    [RequireComponent(typeof(AssistantStateMachine))]
    [RequireComponent(typeof(SearchTargetState))]
    public class Assistant : MonoCache
    {
        [Range(0, 1)] public int AssistantId;

        private AssistantStateMachine _stateMachine;
        private SearchTargetState _searchTargetState;
        
        private List<Vector3> _currentTargetPoints = new();
        private Vector3 _returnPoint = Vector3.zero;
        
        public AssistantData AssistantData { get; private set; }

        public void Construct(AssistantData assistantData, CartridgeGun[] cartridgeGuns, StorageAmmoPlate storageAmmoPlate,
            PoolFreeMoney poolFreeMoney, TriggerReturnMoney triggerReturnMoney)
        {
            AssistantData = assistantData;
            
            if (AssistantId == (int)TypeAssistant.CargoAssistant)
            {
                if (cartridgeGuns.Length != 0)
                {
                    for (int i = 0; i < cartridgeGuns.Length; i++)
                    {
                        if (cartridgeGuns[i].isActiveAndEnabled)
                            _currentTargetPoints.Add(cartridgeGuns[i].transform.position);
                    }

                    _searchTargetState.InjectDirections(_currentTargetPoints, _returnPoint);
                    _stateMachine.EnterBehavior<SearchTargetState>();
                }
                else
                {
                    Debug.Log("Массив платформ для патрон пуст");
                }
            }

            if (AssistantId == (int)TypeAssistant.MoneyAssistant)
            {
                //мысль такая если забуду, так как FSM GENERAL на два типа помощника, то входные точки должны быть две,
                //это лист точек патрулирования и возвратная точка
                //монеты в пули должны инвокаться, и список векторов должен дополняться
                //когда монета подобрана, вектор из листа удаляется
                
                Debug.Log("Здесь еще нет логики");
            }
        }

        private void OnValidate()
        {
            _searchTargetState = Get<SearchTargetState>();
            _stateMachine = Get<AssistantStateMachine>();
        }
    }

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(IdleState))]
    public class AssistantStateMachine : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        [HideInInspector] [SerializeField] private NavMeshAgent _navMeshAgent;

        [HideInInspector] [SerializeField] private IdleState _idleState;

        private Dictionary<Type, ISwitcherState> _allBehaviors;
        private ISwitcherState _currentBehavior;
        private AssistantData _assistant;

        private void OnValidate()
        {
            _assistant = Get<AssistantData>();
            
            _animator = Get<Animator>();
            _navMeshAgent = Get<NavMeshAgent>();

            _idleState = Get<IdleState>();
        }

        private void Start()
        {
            _allBehaviors = new Dictionary<Type, ISwitcherState>
            {
                [typeof(IdleState)] = _idleState
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this, _animator, _navMeshAgent, _assistant);
                behavior.Value.ExitBehavior();
            }

            _currentBehavior = _allBehaviors[typeof(IdleState)];
            EnterBehavior<IdleState>();
        }

        public void EnterBehavior<TState>() where TState : ISwitcherState
        {
            var behavior = _allBehaviors[typeof(TState)];
            _currentBehavior.InActive();
            _currentBehavior.ExitBehavior();
            behavior.EnterBehavior();
            behavior.OnActive();
            _currentBehavior = behavior;
        }
    }

    public class IdleState : State
    {
        public override void OnActive() => 
            AnimatorCached.SetBool(AssistantData.AssistantIdleHash, true);

        public override void InActive() => 
            AnimatorCached.SetBool(AssistantData.AssistantIdleHash, false);
    }

    public class SearchTargetState : State
    {
        public override void OnActive()
        {
        }

        public override void InActive()
        {
        }

        public void InjectDirections(List<Vector3> currentTargetPoints, Vector3 returnPoint)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class State : MonoCache, ISwitcherState
    {
        protected AssistantStateMachine StateMachine;
        protected Animator AnimatorCached;
        protected NavMeshAgent Agent;
        protected AssistantData AssistantData;

        public abstract void OnActive();
        public abstract void InActive();

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(AssistantStateMachine stateMachine, Animator animator, NavMeshAgent navMeshAgent,
            AssistantData assistantData)
        {
            AssistantData = assistantData;
            Agent = navMeshAgent;
            AnimatorCached = animator;
            StateMachine = stateMachine;
        }
    }

    public interface ISwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public abstract void OnActive();
        public abstract void InActive();

        public void Init(AssistantStateMachine stateMachine, Animator animator, NavMeshAgent navMeshAgent,
            AssistantData assistantData);
    }
    
    public class TriggerReturnMoney : MonoCache
    {
    }

    public class PoolFreeMoney
    {
    }
}