using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Behavior_Designer.Runtime.Variables
{
    [Serializable]
    public class SharedVector2 : SharedVariable<Vector2>
    {
        public static implicit operator SharedVector2(Vector2 value) { return new SharedVector2 { Value = value }; }
    }
}