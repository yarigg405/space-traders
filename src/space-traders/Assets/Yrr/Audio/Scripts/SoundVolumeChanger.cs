using System;
using UnityEngine;
using UnityEngine.Audio;


namespace Yrr.Audio
{
    internal sealed class SoundVolumeChanger : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;

        [SerializeField] private VolumeSettings[] _musicSettings;
        [SerializeField] private VolumeSettings[] _soundsSettings;

        internal void SetMusicVolume(float volume)
        {
            for (int i = 0; i < _musicSettings.Length; i++)
            {
                var set = _musicSettings[i];
                var soundValue = ConvertToDb(set.GlobalModificator * volume);
                if (soundValue < -59) soundValue = -59;
                _mixer.SetFloat(set.MixerGroupName, soundValue);
            }
        }

        internal void SetSoundsVolume(float volume)
        {
            for (int i = 0; i < _soundsSettings.Length; i++)
            {
                var set = _soundsSettings[i];
                var soundValue = ConvertToDb(set.GlobalModificator * volume);
                if (soundValue < -59) soundValue = -59;
                _mixer.SetFloat(set.MixerGroupName, soundValue);
            }
        }

        private float ConvertToDb(float volume)
        {
            return Mathf.Log10(volume) * 20;
        }
    }


    [Serializable]
    internal struct VolumeSettings
    {
        [SerializeField] public string MixerGroupName;
        [SerializeField] public float GlobalModificator;
    }
}
