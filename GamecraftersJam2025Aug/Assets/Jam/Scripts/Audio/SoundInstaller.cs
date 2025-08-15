using Jam.Scripts.Audio.Data;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Jam.Scripts.Audio
{
    public class SoundInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixerGroup _musicMixer;
        [SerializeField] private AudioMixerGroup _soundMixer;
        [SerializeField] private SoundRepository _soundRepository;
        
        public override void InstallBindings()
        {
            AudioSettingsInstall();
            SoundServiceInstall();
        }
        
        private void AudioSettingsInstall()
        {
            Container.Bind<PersistentAudioSettings>()
                .FromNew()
                .AsSingle()
                .WithArguments(_musicMixer, _soundMixer)
                .NonLazy();
        }
        
        private void SoundServiceInstall()
        {
            Container.Bind<AudioService>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .WithArguments(_soundRepository)
                .NonLazy();
        }
    }
}
