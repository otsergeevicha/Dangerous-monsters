using System;
using System.Collections.Generic;
using System.Linq;
using ContactPlatforms;
using Plugins.MonoCache;
using SO;
using Turret;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
    [RequireComponent(typeof(AssistantStateMachine))]
    public class CargoAssistant : MonoCache
    {
        private CartridgeGun[] _cartridgeGuns;
        private IdleState _idleState;
        
        public AssistantData AssistantData { get; private set; }

        public void Construct(AssistantData assistantData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate)
        {
            AssistantData = assistantData;
            _cartridgeGuns = cartridgeGuns;

            foreach (CartridgeGun cartridgeGun in _cartridgeGuns)
                cartridgeGun.Activated += UpdateListPoints;
        }

        protected override void OnDisabled()
        {
            foreach (CartridgeGun cartridgeGun in _cartridgeGuns)
                cartridgeGun.Activated -= UpdateListPoints;
        }

        private void OnValidate() => 
            _idleState = Get<IdleState>();

        public void UpdateListPoints() => 
            _idleState.SetActualPoint(_cartridgeGuns.Where(gun => 
                gun.isActiveAndEnabled).ToList());
    }

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(IdleState))]
    [RequireComponent(typeof(CargoAssistant))]
    public class AssistantStateMachine : MonoCache
    {
        [HideInInspector] [SerializeField] private Animator _animator;
        [HideInInspector] [SerializeField] private NavMeshAgent _navMeshAgent;

        [HideInInspector] [SerializeField] private IdleState _idleState;

        private Dictionary<Type, ISwitcherState> _allBehaviors;
        private ISwitcherState _currentBehavior;
        private CargoAssistant _cargoAssistant;

        private void OnValidate()
        {
            _cargoAssistant = Get<CargoAssistant>();
            
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
                behavior.Value.Init(this, _animator, _navMeshAgent, _cargoAssistant.AssistantData);
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
        public override void OnActive()
        {
        }

        public override void InActive()
        {
        }

        public void SetActualPoint(List<CartridgeGun> toList)
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
}