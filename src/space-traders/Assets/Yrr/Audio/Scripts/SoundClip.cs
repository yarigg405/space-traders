using System;
using UnityEngine;


namespace Yrr.Audio
{
    [Serializable]
    internal struct SoundClip
    {
        public AudioClip Clip;
        public float ClipVolume;    //Range: (0f - 2f)     Default: 0.3f
        public float BasePitch;     //Range: (-3f - 3f)    Default: 1f
        public float PitchRandom;   //Range: (0f  -3f)     Default: 0.3f
    }
}
