using Services.Inputs;
using UnityEngine;

namespace Inputs
{
    public class InputService : IInputService
    {
        private readonly MapInputs _input = new ();

         public Vector2 MoveAxis =>
             _input.Player.Move.ReadValue<Vector2>();

         public bool IsCurrentDevice() =>
             _input.KeyboardMouseScheme.name == Constants.KeyboardMouse;
        
         public void OnControls() =>
             _input.Player.Enable();
        
         public void OffControls() =>
             _input.Player.Disable();
    }
}