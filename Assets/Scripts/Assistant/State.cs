using Plugins.MonoCache;
using SO;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
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
}