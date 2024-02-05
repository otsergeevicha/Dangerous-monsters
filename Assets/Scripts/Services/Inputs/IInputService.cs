using UnityEngine;

namespace Services.Inputs
{
    public interface IInputService
    {
        Vector2 MoveAxis { get; }
        bool IsCurrentDevice();
        void OnControls();
        void OffControls();
    }
}