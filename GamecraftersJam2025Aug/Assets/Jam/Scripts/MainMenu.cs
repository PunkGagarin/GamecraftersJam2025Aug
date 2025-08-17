using Jam.Scripts.Audio.View;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.Utils.Coroutine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startGame;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _credits;

        [Inject] private SceneLoader _sceneLoader;
        [Inject] private CoroutineHelper _coroutineHelper;
        [Inject] private AudioSettingsView _audioSettingsView;
        
        private void Awake()
        {
            _startGame.onClick.AddListener(StartGame);
            _settings.onClick.AddListener(OpenSettings);
            _credits.onClick.AddListener(OpenCredits);
        }
        
        private void StartGame()
        {
            _coroutineHelper.RunCoroutine(_sceneLoader.LoadScene(SceneEnum.Gameplay));
        }
        
        private void OpenSettings()
        {
            _audioSettingsView.Open(false);
        }
        
        private void OpenCredits()
        {
            
        }
    }
}
