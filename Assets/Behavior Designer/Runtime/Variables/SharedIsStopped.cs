using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Behavior_Designer.Runtime.Variables
{
    [SerializeField]
    public class SharedIsStopped : SharedVariable<bool>
    {
        public static implicit operator SharedIsStopped(bool value) { return new SharedIsStopped { Value = value }; }
    }
}