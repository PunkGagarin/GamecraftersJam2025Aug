using UnityEngine;

namespace Jam.Scripts.Audio.Data
{
    public class AudioSettingsModel
    {
        public float MasterVolume { get; private set; }
        public float SoundVolume { get; private set; }
        public float MusicVolume { get; private set; }

        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string SOUND_VOLUME_KEY = "SoundVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";

        private const float DEFAULT_MASTER_VOLUME = .5f;
        private const float DEFAULT_SOUND_VOLUME = 1f;
        private const float DEFAULT_MUSIC_VOLUME = 1f;
        
        public AudioSettingsModel() => 
            LoadSettings();

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, SoundVolume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
        }

        public void SetMasterVolume(float volume) => 
            MasterVolume = volume;

        public void SetSoundVolume(float volume) => 
            SoundVolume = volume;

        public void SetMusicVolume(float volume) => 
            MusicVolume = volume;

        private void LoadSettings()
        {
            MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_MASTER_VOLUME);
            SoundVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, DEFAULT_SOUND_VOLUME);
            MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_MUSIC_VOLUME);
        }
    }
}
