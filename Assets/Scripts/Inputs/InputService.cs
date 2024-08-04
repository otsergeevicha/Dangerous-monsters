using System;
using Services.Inputs;
using UnityEngine;

namespace Inputs
{
    public class InputService : IInputService
    {
        private readonly MapInputs _input = new ();

        public event Action OnJoystick;
        public event Action OffJoystick;
        
        public InputService()
        {
            _input.Player.Touch.started += _ =>
                OnJoystick?.Invoke();
            _input.Player.Touch.canceled += _ =>
                OffJoystick?.Invoke();
        }
        
        public Vector2 MoveAxis =>
             _input.Player.Move.ReadValue<Vector2>();

        public Vector2 TouchJoystick => 
            _input.Player.TouchPosition.ReadValue<Vector2>();

        public void OnControls() => 
            _input.Player.Enable();

        public void OffControls() => 
             _input.Player.Disable();
    }
}