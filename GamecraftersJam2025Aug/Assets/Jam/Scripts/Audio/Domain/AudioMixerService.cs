using UnityEngine;
using UnityEngine.Audio;

namespace Jam.Scripts.Audio.Domain
{
    public class AudioMixerService : IAudioMixerService
    {
        public AudioMixerGroup SoundMixer { get; private set; }
        public AudioMixerGroup MusicMixer { get; private set; }
        
        private const string VOLUME = "Volume";

        public AudioMixerService(AudioMixerGroup musicMixer, AudioMixerGroup soundMixer)
        {
            MusicMixer = musicMixer;
            SoundMixer = soundMixer;
        }

        public void SetMasterVolume(float musicVolume, float soundVolume, float newVolume)
        {
            SetMusicVolume(musicVolume, newVolume);
            SetSoundVolume(soundVolume, newVolume);
        }

        public void SetMusicVolume(float newVolume, float masterVolume) => 
            SoundMixer.audioMixer.SetFloat(VOLUME, ToDecibel(newVolume, masterVolume));

        public void SetSoundVolume(float newVolume, float masterVolume) => 
            MusicMixer.audioMixer.SetFloat(VOLUME, ToDecibel(newVolume, masterVolume));

        private float ToDecibel(float newVolume, float masterVolume)
        {
            float volume = newVolume * masterVolume;
            volume = Mathf.Approximately(volume, 0f) ? -80f : Mathf.Log10(volume) * 20f;
            return volume;
        }
    }
}
