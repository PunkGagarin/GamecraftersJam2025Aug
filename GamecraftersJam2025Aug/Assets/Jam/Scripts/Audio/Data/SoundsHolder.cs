using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Audio.Data
{
    [CreateAssetMenu(fileName = "SoundsHolder", menuName = "Game Resources/Audio/SoundsHolder")]
    public class SoundsHolder : ScriptableObject
    {
        [field: SerializeField] public List<AudioClip> Sounds { get; private set; }
    }
}
