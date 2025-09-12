using System;
using System.Collections.Generic;
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
        [SerializeField] private Image _bg;
        
        private bool _isCardSelected;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartClicked);
            _itemContent.gameObject.SetActive(false);
            _bg.gameObject.SetActive(false);
        }

        private void OnStartClicked()
        {
            _startButton.gameObject.SetActive(false);
            _itemContent.gameObject.SetActive(true);
            _bg.gameObject.SetActive(true);
            _presenter.OnStartClicked();
        }

        public void ShowDealEvent(DealUiData data)
        {
            _startButton.image.sprite = data.Icon;
            _startButton.gameObject.SetActive(true);
        }

        public void InitializePrefabs(List<KeyValuePair<DealCardView,DealButtonData>> prefabs)
        {
            foreach (var dealUiData in prefabs)
            {
                var view = Instantiate(dealUiData.Key, _itemContent);
                view.SetData(dealUiData.Value);
                view.OnClick += OnCardClicked;
                view.OnMouseEnter += OnCardMouseEnter;
                view.OnMouseExit += OnCardMouseExit;
            }
        }

        private void OnCardClicked(DealCardView dealCardView, DealButtonData data)
        {
            if (!_isCardSelected)
            {
                dealCardView.RestoreScale();
                HideNonSelectedCards(dealCardView);
                _presenter.OnCardSelected(data);
            }
            _isCardSelected = true;
        }

        private void HideNonSelectedCards(DealCardView dealCardView)
        {
            foreach (Transform child in _itemContent)
            {
                if (child.TryGetComponent(out DealCardView view)) 
                    view.gameObject.SetActive(dealCardView == view);
            }
        }

        private void OnCardMouseEnter(DealCardView view)
        {
            if (!_isCardSelected)
                view.ScaleUp();
        }

        private void OnCardMouseExit(DealCardView view)
        {
            if (!_isCardSelected)
                view.RestoreScale();
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
            foreach (Transform child in _itemContent)
            {
                if (child.TryGetComponent(out DealCardView view))
                {
                    view.OnClick -= OnCardClicked;
                    view.OnMouseEnter -= OnCardMouseEnter;
                    view.OnMouseExit -= OnCardMouseExit;
                }
            }
        }
    }
}