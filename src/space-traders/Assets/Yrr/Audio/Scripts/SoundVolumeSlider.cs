using System;
using UnityEngine;
using UnityEngine.UI;


namespace Yrr.Audio
{
    internal sealed class SoundVolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private SoundVolumeChanger _volumeChanger;

        private void OnEnable()
        {
            _volumeSlider.onValueChanged.AddListener(UpdateVolume);
            UpdateVolume(_volumeSlider.value);
        }

        private void OnDisable()
        {
            _volumeSlider.onValueChanged.RemoveListener(UpdateVolume);
        }

        private void UpdateVolume(float arg0)
        {
            _volumeChanger.SetSoundsVolume(arg0);
        }
    }
}