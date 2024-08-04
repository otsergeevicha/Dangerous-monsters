using System;
using UnityEngine;

namespace Services.Inputs
{
    public interface IInputService
    {
        event Action OnJoystick;
        event Action OffJoystick;
        Vector2 MoveAxis { get; }
        Vector2 TouchJoystick { get; }
        void OnControls();
        void OffControls();
    }
}