using Jam.Scripts.Audio.Data;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Jam.Scripts.Audio.Domain
{
    public class SoundInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixerGroup _musicMixer;
        [SerializeField] private AudioMixerGroup _soundMixer;
        [SerializeField] private SoundRepository _soundRepository;
        
        public override void InstallBindings()
        {
            AudioSettingsModelInstall();
            AudioMixerInstall();
            AudioSettingsPresenterInstall();
            SoundServiceInstall();
        }

        private void AudioSettingsModelInstall()
        {
            Container.Bind<AudioSettingsModel>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void AudioMixerInstall()
        {
            Container.BindInterfacesAndSelfTo<AudioMixerService>()
                .FromNew()
                .AsSingle()
                .WithArguments(_musicMixer, _soundMixer)
                .NonLazy();
        }
        
        private void AudioSettingsPresenterInstall()
        {
            Container.BindInterfacesAndSelfTo<AudioSettingsPresenter>()
                .FromNew()
                .AsSingle()
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
