using Jam.Scripts.Utils.Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts
{
    public class GameplayUi : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [Inject] private PopupManager _popupManager;
    
        private void Awake()
        {
            _pauseButton.onClick.AddListener(OpenPausePopup);
        }

        private void OpenPausePopup()
        {
            _popupManager.OpenPopup<PausePopup>(null, null, true);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OpenPausePopup);
        }
    }
}
