using System;
using Inputs;
using Plugins.MonoCache;
using Services.Inputs;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Hero))]
    [RequireComponent(typeof(Animator))]
    public class HeroMovement : MonoCache
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _controller;
        private IInputService _input;

        public void Construct(IInputService input)
        {
            _input = input;
            _input.OnControls();
        }

        private void OnValidate()
        {
            _controller = Get<CharacterController>();
            _animator = Get<Animator>();
        }

        protected override void UpdateCached() => 
            BaseLogic();

        protected override void OnDisabled() => 
            _input.OffControls();

        private void BaseLogic()
        {
            Vector3 movementDirection = Vector3.zero;

            if (_input.MoveAxis.sqrMagnitude > Single.Epsilon)
            {
                _animator.SetBool(Constants.HeroRunHash, true);

                movementDirection = new Vector3(_input.MoveAxis.x,Single.Epsilon, _input.MoveAxis.y);
                
                if (movementDirection != Vector3.zero) 
                    transform.forward = movementDirection.normalized;
            }
            else
            {
                _animator.SetBool(Constants.HeroRunHash, false);
                _animator.SetBool(Constants.HeroIdleHash, true);
            }
            
            movementDirection += Physics.gravity;

            _controller.Move(movementDirection * (Constants.HeroSpeed * Time.deltaTime));
        }
    }
}