using Jam.Scripts.Audio.Data;
using Jam.Scripts.Audio.View;
using Zenject;

namespace Jam.Scripts.Audio.Domain
{
    public class AudioSettingsPresenter : IInitializable
    {
        private AudioSettingsModel _audioSettingsModel;
        private IAudioMixerService _audioMixerService;
        private AudioSettingsView _audioSettingsView;

        private float prevMasterVolume, prevMusicVolume, prevSfxVolume;

        public AudioSettingsPresenter(AudioSettingsModel audioSettingsModel, IAudioMixerService audioMixerService)
        {
            _audioSettingsModel = audioSettingsModel;
            _audioMixerService = audioMixerService;
        }

        public void Initialize()
        {
            _audioMixerService.SetMasterVolume(_audioSettingsModel.MusicVolume, _audioSettingsModel.SoundVolume,
                _audioSettingsModel.MasterVolume);
        }

        public void AttachView(AudioSettingsView view)
        {
            if (_audioSettingsView != null)
                return;

            _audioSettingsView = view;
            SyncWithView();
        }

        public void SetMasterVolume(float volume)
        {
            _audioSettingsModel.SetMasterVolume(volume);
            UpdateMasterVolume(volume);
        }

        public void SetSoundVolume(float volume)
        {
            _audioSettingsModel.SetSoundVolume(volume);
            UpdateSoundVolume(volume);
        }

        public void SetMusicVolume(float volume)
        {
            _audioSettingsModel.SetMusicVolume(volume);
            UpdateMusicVolume(volume);
        }

        public void UndoChanges()
        {
            SetMasterVolume(prevMasterVolume);
            SetMusicVolume(prevMusicVolume);
            SetSoundVolume(prevSfxVolume);
            Initialize();
            SyncWithView();
        }

        public void SaveChanges() => _audioSettingsModel.SaveSettings();

        public void OnOpen()
        {
            prevMasterVolume = _audioSettingsModel.MasterVolume;
            prevMusicVolume = _audioSettingsModel.MusicVolume;
            prevSfxVolume = _audioSettingsModel.SoundVolume;
            SyncWithView();
        }

        private void UpdateMasterVolume(float newVolume) =>
            _audioMixerService.SetMasterVolume(_audioSettingsModel.MusicVolume, _audioSettingsModel.SoundVolume,
                newVolume);

        private void UpdateSoundVolume(float newVolume) =>
            _audioMixerService.SetSoundVolume(newVolume, _audioSettingsModel.MasterVolume);

        private void UpdateMusicVolume(float newVolume) =>
            _audioMixerService.SetMusicVolume(newVolume, _audioSettingsModel.MasterVolume);


        private void SyncWithView()
        {
            _audioSettingsView.SetMasterVolume(_audioSettingsModel.MasterVolume);
            _audioSettingsView.SetSoundVolume(_audioSettingsModel.SoundVolume);
            _audioSettingsView.SetMusicVolume(_audioSettingsModel.MusicVolume);
        }
    }
}