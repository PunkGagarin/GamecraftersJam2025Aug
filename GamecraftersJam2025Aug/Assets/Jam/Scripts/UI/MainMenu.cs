using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Audio.View;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.Utils.Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startGame;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _credits;

        [Inject] private SceneChanger _sceneChanger;
        [Inject] private PopupManager _popupManager;
        [Inject] private AudioService _audioService;

        private void Awake()
        {
            _startGame.onClick.AddListener(StartGame);
            _settings.onClick.AddListener(OpenSettings);
            _credits.onClick.AddListener(OpenCredits);
            StartBgm();
        }

        private void StartBgm()
        {
            _audioService.PlayMusic(Sounds.mainMenuBgm.ToString());
        }

        private void OnDestroy()
        {
            _startGame.onClick.RemoveListener(StartGame);
            _settings.onClick.RemoveListener(OpenSettings);
            _credits.onClick.RemoveListener(OpenCredits);
        }

        private void StartGame()
        {
            _audioService.PlaySound(Sounds.buttonClick);
            _sceneChanger.StartGameplay();
        }

        private void OpenSettings()
        {
            _audioService.PlaySound(Sounds.buttonClick);
            _popupManager.OpenPopup<SettingsView>();
        }

        private void OpenCredits()
        {
            _audioService.PlaySound(Sounds.buttonClick);
            _popupManager.OpenPopup<CreditsPopup>();
        }
    }
}