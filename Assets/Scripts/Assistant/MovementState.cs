using UnityEngine;

namespace Assistant
{
    public class MovementState : State
    {
        private Vector3 _endedPoint;

        public override void OnActive()
        {
            AnimatorCached.SetBool(AssistantData.RunHash, true);
            Agent.Move(_endedPoint);
        }

        public override void InActive()
        {
            AnimatorCached.SetBool(AssistantData.RunHash, false);
            Agent.Stop();
        }

        public void SetPoint(Vector3 newPoint) =>
            _endedPoint = newPoint;
    }
}