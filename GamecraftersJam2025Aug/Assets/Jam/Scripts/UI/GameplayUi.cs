using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Utils.Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.UI
{
    public class GameplayUi : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [Inject] private PopupManager _popupManager;
        [Inject] private AudioService _audioService;

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OpenPausePopup);
            _audioService.PlayMusic(Sounds.gameplayBgm.ToString(), true);
        }

        private void OpenPausePopup()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _popupManager.OpenPopup<PausePopup>();
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OpenPausePopup);
        }
    }
}