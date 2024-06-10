using Plugins.MonoCache;
using UnityEngine;
using UnityEngine.UI;

namespace HpBar
{
    public class HealthBar : MonoCache
    {
        [SerializeField] private Image _image;

        private Transform _followingTransform;

        private float _currentFillAmount = 1f;
        private float _targetFillAmount;
        private bool _isChange;

        public void Construct(Transform following)
        {
            _followingTransform = following;
        }

        protected override void UpdateCached()
        {
            transform.position = new Vector3(_followingTransform.position.x, 2.5f, _followingTransform.position.z);

            if (_isChange)
            {
                _currentFillAmount -= Time.deltaTime;
                _image.fillAmount = _currentFillAmount;
            
                if (_currentFillAmount <= _targetFillAmount) 
                    _isChange = false;
            }
        }

        public void ChangeValue(float current, float max)
        {
            _isChange = true;
            _targetFillAmount = current / max;
        }
    }
}