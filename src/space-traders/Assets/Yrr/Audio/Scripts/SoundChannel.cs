using Assets.Yrr.Audio.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Yrr.Utils;


namespace Yrr.Audio
{
    internal sealed class SoundChannel : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixerGroup;
        [SerializeField] private SoundPool _pool;
        [SerializeField] private SoundClipsStorage _sounds;
        [SerializeField] private int _maxSounds = 10;

        [SerializeField] private float _fadeInTime;
        [SerializeField] private float _fadeOutTime;

        [SerializeField] private bool _loopSound;

        private readonly List<AudioSource> _playingSounds = new();
        private readonly List<AudioSource> _notActiveSounds = new();
        private readonly List<AudioSource> _removableSounds = new();

        private AudioListener m_listener;
        private AudioListener _listener
        {
            get
            {
                if (m_listener == null || !m_listener.isActiveAndEnabled)
                {
                    m_listener = FindAnyObjectByType<AudioListener>();
                }
                return m_listener;
            }
        }


        private void Update()
        {
            foreach (var sound in _playingSounds)
            {
                if (!sound || !sound.loop && sound.time >= sound.clip.length || !sound.isPlaying)
                {
                    _removableSounds.Add(sound);
                }
            }

            foreach (var sound in _notActiveSounds)
            {
                if (!sound || (!sound.loop && (sound.time >= sound.clip.length || !sound.isPlaying)))
                {
                    _removableSounds.Add(sound);
                }
            }

            if (_removableSounds.Count == 0) return;

            foreach (var sound in _removableSounds)
            {
                _playingSounds.Remove(sound);
                _notActiveSounds.Remove(sound);
                _pool.DespawnObject(sound);
            }

            _removableSounds.Clear();
        }


        public void PlaySoundOnCamera(string soundId, float startPlayTime = 0)
        {
            var source = BuildSource(soundId);
            source.time = startPlayTime;
            PlayOnObject(source, _listener.transform);
        }

        public void PlaySoundOnCamera(AudioClip clip)
        {
            var source = BuildSource(clip);
            PlayOnObject(source, _listener.transform);
        }


        public void PlaySoundOnObject(string soundId, Transform tr)
        {
            var source = BuildSource(soundId);
            PlayOnObject(source, tr);
        }

        public void PlaySoundOnObject(AudioClip clip, Transform tr)
        {
            var source = BuildSource(clip);
            PlayOnObject(source, tr);
        }


        public void PlaySoundInPosition(string soundId, Vector3 position)
        {
            var source = BuildSource(soundId);
            PlayInPosition(source, position);
        }

        public void PlaySoundInPosition(AudioClip clip, Vector3 position)
        {
            var source = BuildSource(clip);
            PlayInPosition(source, position);
        }



        private AudioSource BuildSource(string soundId)
        {
            var sound = _sounds.Get(soundId).GetRandomItem();
            var source = _pool.SpawnObject();
            _playingSounds.Add(source);

            source.clip = sound.Clip;
            source.loop = _loopSound;
            source.outputAudioMixerGroup = _mixerGroup;
            source.pitch = sound.BasePitch + Random.Range(0f, sound.PitchRandom);

            if (_fadeInTime > 0)
            {
                source.volume = 0.01f;
                ChangeVolume(source, sound.ClipVolume, _fadeInTime);
            }
            else
            {
                source.volume = sound.ClipVolume;
            }

            return source;
        }

        private AudioSource BuildSource(AudioClip clip)
        {
            var source = _pool.SpawnObject();
            _playingSounds.Add(source);

            source.clip = clip;
            source.loop = _loopSound;
            source.outputAudioMixerGroup = _mixerGroup;
            source.pitch = 1f;

            if (_fadeInTime > 0)
            {
                source.volume = 0.01f;
                ChangeVolume(source, 1f, _fadeInTime);
            }
            else
            {
                source.volume = 1f;
            }

            return source;
        }


        private void PlayOnObject(AudioSource source, Transform parent)
        {
            RestrictSoundsCount();
            var tr = source.transform;
            tr.SetParent(parent);
            tr.localPosition = Vector3.zero;
            source.Play();
        }

        private void PlayInPosition(AudioSource source, Vector3 position)
        {
            RestrictSoundsCount();
            var tr = source.transform;
            tr.SetParent(null);
            tr.position = position;
            source.Play();
        }

        private void RestrictSoundsCount()
        {
            if (_playingSounds.Count <= _maxSounds)
                return;

            var first = _playingSounds.First();

            if (_fadeOutTime > 0)
            {
                ChangeVolume(first, 0f, _fadeOutTime, true);
            }

            else
            {
                first.volume = 0f;
                first.Stop();
            }

            _playingSounds.Remove(first);
            _notActiveSounds.Add(first);
        }


        private void ChangeVolume(AudioSource source, float targetVolume, float fadingDuration, bool killOnEnd = false)
        {
            if (fadingDuration <= 0f)
            {
                source.volume = targetVolume;
                if (killOnEnd)
                {
                    source.Stop();
                }
            }

            else
            {
                StartCoroutine(SmoothChangeVolume(source, source.volume, targetVolume, fadingDuration, killOnEnd));
            }
        }

        private IEnumerator SmoothChangeVolume(AudioSource source,
            float startVolume, float targetVolume, float fadingDuration,
            bool killOnEnd = false)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadingDuration)
            {
                var t = elapsedTime / fadingDuration;
                var currentValue = Mathf.Lerp(startVolume, targetVolume, t);
                source.volume = currentValue;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            source.volume = targetVolume;

            if (killOnEnd)
            {
                source.Stop();
            }
        }

        internal void StopAll()
        {
            foreach (var sound in _playingSounds)
            {

                ChangeVolume(sound, 0f, _fadeOutTime, true);
            }
        }
    }
}
