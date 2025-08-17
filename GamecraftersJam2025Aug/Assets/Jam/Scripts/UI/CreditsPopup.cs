using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Utils.Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts
{
    public class CreditsPopup : Popup
    {
        [SerializeField] private Button _closeButton;

        [Inject] private AudioService _audioService;
        private void Awake() => _closeButton.onClick.AddListener(Close);

        private void OnDestroy() => _closeButton.onClick.RemoveListener(Close);

        public override void Close()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            base.Close();
        }
    }
}