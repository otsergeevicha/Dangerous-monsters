using System;
using Plugins.MonoCache;
using UnityEngine;

namespace HpBar
{
    public class HealthBar : MonoCache
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _image;

        private Transform _followingTransform;

        private float _targetFillAmount;
        private bool _isChange;
        private bool _isLowHp;
        private float _waitTime = 2f;

        public void Construct(Transform following)
        {
            _followingTransform = following;
            _mainCanvas.enabled = false;
        }

        protected override void UpdateCached()
        {
            transform.position = new Vector3(_followingTransform.position.x, 2.5f, _followingTransform.position.z);

            if (_isChange)
            {
                _mainCanvas.enabled = true;

                var temp = _image.anchorMax;
                temp.x -= Time.deltaTime / 2;
                _image.anchorMax = temp;

                if (temp.x <= _targetFillAmount)
                {
                    _isChange = false;
                    _isLowHp = true;
                }
            }

            if (_isLowHp)
            {
                _waitTime -= Time.deltaTime / 2;
                _canvasGroup.alpha = _waitTime;

                if (_waitTime <= Single.Epsilon)
                {
                    _mainCanvas.enabled = false;
                    _isLowHp = false;
                    _canvasGroup.alpha = 1;
                }
            }
        }

        public void ChangeValue(float current, float max)
        {
            _isChange = true;
            _targetFillAmount = current / max;
        }
    }
}