using System;
using UnityEngine;

namespace Jam.Scripts.Audio.Data
{
    [Serializable]
    public class SoundElement
    {
        [SerializeField, Range(0, 1f)] private float _volume = 1f;
        [SerializeField] private AudioClip _clip;

        public SoundElement(AudioClip clip)
        {
            _clip = clip;
        }
        
        public float Volume => _volume;

        public AudioClip Clip => _clip;
    }
}