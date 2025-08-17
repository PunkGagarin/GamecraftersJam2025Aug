using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Utils.Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Audio.View
{
    public class AudioSettingsView : Popup
    {
        [SerializeField] private Slider _masterVolumeSlider, _musicVolumeSlider, _soundVolumeSlider;
        [SerializeField] private Button _closeButton, _applyButton;

        [Inject] private AudioSettingsPresenter _audioSettingsPresenter;
        [Inject] private AudioService _audioService;

        private void Awake()
        {
            _masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
            _musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
            _soundVolumeSlider.onValueChanged.AddListener(UpdateSoundVolume);
            _closeButton.onClick.AddListener(UndoChanges);
            _applyButton.onClick.AddListener(SaveSettings);
        }

        private void SaveSettings()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _audioSettingsPresenter.SaveChanges();
            Close();
        }

        private void UndoChanges()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _audioSettingsPresenter.UndoChanges();
            Close();
        }

        public override void Open(bool withPause)
        {
            _audioSettingsPresenter.AttachView(this);
            _audioSettingsPresenter.OnOpen();
            base.Open(withPause);
        }

        private void OnDestroy()
        {
            _masterVolumeSlider.onValueChanged.RemoveListener(UpdateMasterVolume);
            _musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
            _soundVolumeSlider.onValueChanged.RemoveListener(UpdateSoundVolume);
            _closeButton.onClick.RemoveListener(UndoChanges);
            _applyButton.onClick.RemoveListener(SaveSettings);
        }

        private void UpdateSoundVolume(float newVolume)
        {
            _audioService.PlaySoundInSingleAudioSource(Sounds.buttonClickShortHigh.ToString());
            _audioSettingsPresenter.SetSoundVolume(newVolume);
        }

        private void UpdateMusicVolume(float newVolume)
        {
            _audioService.PlaySoundInSingleAudioSource(Sounds.buttonClickShortHigh.ToString());
            _audioSettingsPresenter.SetMusicVolume(newVolume);
        }

        private void UpdateMasterVolume(float newVolume)
        {
            _audioService.PlaySoundInSingleAudioSource(Sounds.buttonClickShortHigh.ToString());
            _audioSettingsPresenter.SetMasterVolume(newVolume);
        }

        public void SetMasterVolume(float masterVolume) =>
            _masterVolumeSlider.value = masterVolume;

        public void SetSoundVolume(float soundVolume) =>
            _soundVolumeSlider.value = soundVolume;

        public void SetMusicVolume(float musicVolume) =>
            _musicVolumeSlider.value = musicVolume;
    }
}