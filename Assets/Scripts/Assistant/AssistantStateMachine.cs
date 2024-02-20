using System;
using System.Collections.Generic;
using Plugins.MonoCache;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(IdleState))]
    [RequireComponent(typeof(CargoAssistant))]
    public class AssistantStateMachine : MonoCache
    {
        [HideInInspector] [SerializeField] private CargoAssistant _cargoAssistant;
        [HideInInspector] [SerializeField] private Animator _animator;
        [HideInInspector] [SerializeField] private NavMeshAgent _navMeshAgent;

        [HideInInspector] [SerializeField] private IdleState _idleState;
        [HideInInspector] [SerializeField] private MovementState _movementState;

        private Dictionary<Type, ISwitcherState> _allBehaviors;
        private ISwitcherState _currentBehavior;

        private void OnValidate()
        {
            _cargoAssistant = Get<CargoAssistant>();

            _animator = Get<Animator>();
            _navMeshAgent = Get<NavMeshAgent>();

            _idleState = Get<IdleState>();
            _movementState = Get<MovementState>();
        }

        private void Start()
        {
            _allBehaviors = new Dictionary<Type, ISwitcherState>
            {
                [typeof(IdleState)] = _idleState,
                [typeof(MovementState)] = _movementState
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
}