using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Audio.Data
{
    [Serializable]
    public class SoundData
    {
        [SerializeField] private string _name;
        [SerializeField] private List<SoundElement> _clips;

        public SoundData(string name, List<SoundElement> clips)
        {
            _name = name;
            _clips = clips;
        }
        public string Name => _name;
        public List<SoundElement> Clips => _clips;
    }
}