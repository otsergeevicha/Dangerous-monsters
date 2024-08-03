using System;
using Services.Inputs;
using UnityEngine;

namespace Inputs
{
    public class InputService : IInputService
    {
        private readonly MapInputs _input = new ();

        public InputService()
        {
            _input.Player.Joystick.started += _ => OnControls();
        }

        public Vector2 MoveAxis =>
             _input.Player.Move.ReadValue<Vector2>();

        public Vector2 TouchJoystick => 
            _input.Player.Joystick.ReadValue<Vector2>();

        public void OnControls() => 
            _input.Player.Enable();

        public void OffControls() => 
             _input.Player.Disable();
    }
}