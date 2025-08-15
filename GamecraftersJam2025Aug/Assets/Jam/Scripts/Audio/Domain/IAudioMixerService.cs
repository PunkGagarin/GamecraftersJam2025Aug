using UnityEngine.Audio;

namespace Jam.Scripts.Audio.Domain
{
    public interface IAudioMixerService
    {
        AudioMixerGroup MusicMixer { get; }
        AudioMixerGroup SoundMixer { get; }
        void SetMasterVolume(float musicVolume, float soundVolume, float newVolume);
        void SetMusicVolume(float newVolume, float masterVolume);
        void SetSoundVolume(float newVolume, float masterVolume);
    }
}
