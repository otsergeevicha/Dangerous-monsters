﻿using System;
using Plugins.MonoCache;
using UnityEngine;

namespace Canvases
{
    public class WinScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private GameObject _buttonContinue;

        private void Start()
        {
            _canvas.enabled = false;
            _particle.gameObject.SetActive(false);
            _buttonContinue.SetActive(false);
        }

        private void OnValidate() => 
            _canvas ??= Get<Canvas>();

        public void OnActive()
        {
            Time.timeScale = 0;
            _particle.gameObject.SetActive(true);
            _canvas.enabled = true;
            SetCursorState(false);
        }

        public void InActive()
        {
            Time.timeScale = 1;
            _particle.gameObject.SetActive(false);
            _buttonContinue.SetActive(false);
            _canvas.enabled = false;
            SetCursorState(true);
        }
        
        private void SetCursorState(bool newState) =>
            Cursor.lockState = newState
                ? CursorLockMode.Locked
                : CursorLockMode.None;

        public void ActiveButtonContinue() => 
            _buttonContinue.SetActive(true);
    }
}