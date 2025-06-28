using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Yrr.UI.Elements
{
    public sealed class SmoothSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private bool _onlyIncrement;
        [SerializeField] private float _speedModifier = 1f;

        private float _targetValue;
        private bool _isMoving;

        public UnityEvent OnSliderMoveUp;
        public UnityEvent OnSliderMoveDown;
        public UnityEvent OnSliderStop;
        public UnityEvent OnSliderMaxReached;

        private void Update()
        {
            if (_slider.value.Equals(_targetValue)) return;

            if (!_isMoving)
            {
                if (_targetValue > _slider.value)
                    OnSliderMoveUp?.Invoke();

                else
                    OnSliderMoveDown?.Invoke();
                _isMoving = true;
            }

            var delta = Mathf.Abs(_slider.value - _targetValue) * 2;
            if (delta < 0.45) delta = 0.45f;
            _slider.value = Mathf.MoveTowards(_slider.value, _targetValue, delta * Time.unscaledDeltaTime * _speedModifier);

            if (!_slider.value.Equals(_targetValue)) return;
            _isMoving = false;
            OnSliderStop?.Invoke();

            if (_slider.value == 1)
                OnSliderMaxReached?.Invoke();
        }



        /// <summary>
        /// Value must be in range 0-1
        /// </summary>
        /// <param name="value"></param>
        public void InitValue(float value)
        {
            if (value < 0)
            {
                value = 0;
            }
            if (value > 1)
            {
                value = 1;
            }

            _targetValue = value;
            _slider.value = value;
        }

        /// <summary>
        /// Value must be in range 0-1
        /// </summary>
        /// <param name="value"></param>
        public void ChangeValue(float value)
        {
            if (value < 0)
            {
                value = 0;
            }
            if (value > 1)
            {
                value = 1;
            }

            if (_onlyIncrement && value == 1)
            {
                OnSliderStop.AddListener(ResetSlider);
            }

            if (value < _targetValue && _onlyIncrement)
            {
                _targetValue = 1;
            }
            else
            {
                _targetValue = value;
            }
        }

        private void ResetSlider()
        {
            _targetValue = 0;
            OnSliderStop.RemoveListener(ResetSlider);
        }
    }
}