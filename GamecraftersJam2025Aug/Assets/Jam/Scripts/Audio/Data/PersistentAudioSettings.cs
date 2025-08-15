using UnityEngine;
using UnityEngine.Audio;

namespace Jam.Scripts.Audio.Data
{
    public class PersistentAudioSettings
    {
        private AudioMixerGroup _musicMixer;
        private AudioMixerGroup _soundMixer;
        public float MasterVolume { get; private set; }
        public float SoundVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public AudioMixerGroup SoundMixer => _soundMixer;
        public AudioMixerGroup MusicMixer => _musicMixer;

        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string SOUND_VOLUME_KEY = "SoundVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string VOLUME = "Volume";

        private const float MASTER_SOUND_VOLUME = .5f;
        private const float DEFAULT_SOUND_VOLUME = 1f;
        private const float DEFAULT_MUSIC_VOLUME = 1f;
        
        public PersistentAudioSettings(AudioMixerGroup musicMixer, AudioMixerGroup soundMixer)
        {
            _musicMixer = musicMixer;
            _soundMixer = soundMixer;
            MasterVolume = PlayerPrefs.HasKey(MASTER_VOLUME_KEY) ? PlayerPrefs.GetFloat(MASTER_VOLUME_KEY) : MASTER_SOUND_VOLUME;
            SoundVolume = PlayerPrefs.HasKey(SOUND_VOLUME_KEY) ? PlayerPrefs.GetFloat(SOUND_VOLUME_KEY) : DEFAULT_SOUND_VOLUME;
            MusicVolume = PlayerPrefs.HasKey(MUSIC_VOLUME_KEY) ? PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY) : DEFAULT_MUSIC_VOLUME;
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, SoundVolume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
        }

        public void SetMasterVolume(float volume)
        {
            MasterVolume = volume;
            UpdateMasterVolume();
        }
        
        public void SetSoundVolume(float volume)
        {
            SoundVolume = volume;
            UpdateSoundVolume(SoundVolume);
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
            UpdateMusicVolume(MusicVolume);
        }
        
        private void UpdateMasterVolume()
        {
            UpdateSoundVolume(SoundVolume);
            UpdateMusicVolume(MusicVolume);
        }

        private void UpdateSoundVolume(float newVolume) => 
            _soundMixer.audioMixer.SetFloat(VOLUME, ToDecibel(newVolume));

        private void UpdateMusicVolume(float newVolume) => 
            _musicMixer.audioMixer.SetFloat(VOLUME, ToDecibel(newVolume));
        
        private float ToDecibel(float newVolume)
        {
            float volume = newVolume * MasterVolume;
            volume = Mathf.Approximately(volume, 0f) ? -80f : Mathf.Log10(volume) * 20f;
            return volume;
        }
    }
}
