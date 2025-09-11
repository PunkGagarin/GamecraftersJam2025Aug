using System;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomDealEventView : ContentUi
    {
        [Inject] private RoomDealEventPresenter _presenter;
        
        [SerializeField] private Button _startButton;
        [SerializeField] private RectTransform _itemContent;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartClicked);
            _startButton.gameObject.SetActive(false);
            _itemContent.gameObject.SetActive(false);
        }

        private void OnStartClicked()
        {
            _startButton.gameObject.SetActive(false);
            _itemContent.gameObject.SetActive(true);
            // create content
        }

        public void ShowDealEvent(DealUiData data)
        {
            _startButton.image.sprite = data.Icon;
            _startButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
        }
    }
}