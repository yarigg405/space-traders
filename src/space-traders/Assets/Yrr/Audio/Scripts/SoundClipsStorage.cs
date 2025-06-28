using System.Collections.Generic;
using UnityEngine;
using Yrr.Audio;


namespace Assets.Yrr.Audio.Scripts
{
    internal sealed class SoundClipsStorage : MonoBehaviour
    {
        [SerializeField] private SoundCatalog[] _soundCatalogs;
        private Dictionary<string, SoundClip[]> _storedClips = new();
        private bool _isInited;

        private void Awake()
        {
            if (!_isInited)
                Init();
        }

        private void Init()
        {
            foreach (var catalog in _soundCatalogs)
            {
                for (int i = 0; i < catalog.Sounds.Length; i++)
                {
                    var pair = catalog.Sounds[i];
                    if (!_storedClips.TryAdd(pair.Name, pair.SoundClip))
                    {
                        Debug.LogError($"Sound with name already added: \"{pair.Name}\". From catalog: {catalog.name}");
                    }
                }
            }

            _isInited = true;
        }

        internal SoundClip[] Get(string soundId)
        {
            return _storedClips[soundId];
        }
    }
}
