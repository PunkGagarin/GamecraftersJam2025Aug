using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Audio.Data
{
    [CreateAssetMenu(fileName = "SoundRepository", menuName = "Audio/SoundRepository")]
    public class SoundRepository : ScriptableObject
    {
        [SerializeField] private SoundsHolder _soundsHolder;
        [SerializeField] private SoundsHolder _musicHolder;
        [field: SerializeField] public List<SoundData> Sounds { get; private set; }
        [field: SerializeField] public List<SoundData> Music { get; private set; }
        
        public SoundElement GetClip(string clipName, SoundType soundType)
        {
            SoundElement clipToReturn = null;
            List<SoundData> soundsData = soundType == SoundType.Effect ? Sounds : Music;
            foreach (SoundData clipData in soundsData)
            {
                if (clipData.Name.Equals(clipName, StringComparison.InvariantCultureIgnoreCase))
                {
                    clipToReturn = clipData.Clips[Random.Range(0, clipData.Clips.Count)];
                    break;
                }
            }

            if (clipToReturn == null)
            {
                Debug.LogError("Clip not found: " + clipName);  
                return null;
            }

            return clipToReturn;
        }

        [Button]
        private void GenerateSounds() =>
            UpdateSoundData(_soundsHolder, SoundType.Effect);

        [Button]
        private void GenerateMusic() =>
            UpdateSoundData(_musicHolder, SoundType.Music);
        
        private void UpdateSoundData(SoundsHolder holder, SoundType soundType)
        {
            if (holder ==null || holder.Sounds.Count == 0)
            {
                Debug.LogError("Сначала надо закинуть SoundsHolder и заполнить его треками!");
                return;
            }

            if (soundType == SoundType.Effect)
                Sounds = new List<SoundData>();
            else 
                Music = new List<SoundData>();
            
            foreach (AudioClip soundClip in holder.Sounds)
            {
                List<SoundElement> clip = new List<SoundElement>{ new SoundElement(soundClip) };
                SoundData clipData = new SoundData(soundClip.name, clip);
                
                if (soundType == SoundType.Effect)
                    Sounds.Add(clipData);
                else 
                    Music.Add(clipData);
            }
        }
    }
}
