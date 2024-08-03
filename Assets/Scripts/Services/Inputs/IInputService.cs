using UnityEngine;

namespace Services.Inputs
{
    public interface IInputService
    {
        Vector2 MoveAxis { get; }
        Vector2 TouchJoystick { get; }
        void OnControls();
        void OffControls();
    }
}