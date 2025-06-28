using System;
using UnityEngine;


namespace Yrr.Audio
{
    [CreateAssetMenu(fileName = "SoundCatalog", menuName = "ScriptableObjects/SoundCatalog", order = 55)]
    internal sealed class SoundCatalog : ScriptableObject
    {
        [field: SerializeField] public SoundNameKeyValuePair[] Sounds { get; private set; }
    }

    [Serializable]
    internal class SoundNameKeyValuePair
    {
        public string Name;
        public SoundClip[] SoundClip;
    }
}
