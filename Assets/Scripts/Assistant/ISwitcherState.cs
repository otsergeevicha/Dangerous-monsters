using SO;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
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