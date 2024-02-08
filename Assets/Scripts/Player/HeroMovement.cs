using System;
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
        
        private readonly float _rotationSpeed = 5;
        private readonly float _maxMagnitudeDelta = 0.0f;
        
        private IInputService _input;
        private int _heroSpeed;
        private int _idleHash;
        private int _runHash;

        public void Construct(IInputService input, int heroSpeed, int idleHash, int runHash)
        {
            _runHash = runHash;
            _idleHash = idleHash;
            _heroSpeed = heroSpeed;
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
                _animator.SetBool(_runHash, true);

                movementDirection = new Vector3(_input.MoveAxis.x, Single.Epsilon, _input.MoveAxis.y);

                if (movementDirection != Vector3.zero)
                {
                    Vector3 targetDirection = movementDirection.normalized;
                    Rotate(targetDirection);
                }
            }
            else
            {
                _animator.SetBool(_runHash, false);
                _animator.SetBool(_idleHash, true);
            }

            movementDirection += Physics.gravity;

            _controller.Move(movementDirection * (_heroSpeed * Time.deltaTime));
        }

        private void Rotate(Vector3 targetDirection)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotationSpeed * Time.deltaTime,
                _maxMagnitudeDelta);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}