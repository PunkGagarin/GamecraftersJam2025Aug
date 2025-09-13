using System.Collections.Generic;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomDealEventView : ContentUi
    {
        [Inject] private RoomDealEventPresenter _presenter;

        [SerializeField] private Button _startButton;
        [SerializeField] private RectTransform _itemContent;
        [SerializeField] private GameObject _text;
        [SerializeField] private Image _bg;
        [SerializeField] private List<DealCardView> _dealCardViewList;

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

        public void Initialize(List<DealButtonData> data)
        {
            for (var index = 0; index < data.Count; index++)
            {
                var cardData = data[index];
                var dealCardView = _dealCardViewList[index];
                dealCardView.gameObject.SetActive(true);
                dealCardView.SetData(cardData);
                dealCardView.OnClick += OnCardClicked;
                dealCardView.OnMouseEnter += OnCardMouseEnter;
                dealCardView.OnMouseExit += OnCardMouseExit;
            }
        }

        private void OnCardClicked(DealCardView dealCardView, DealButtonData data)
        {
            _text.SetActive(false);
            dealCardView.RestoreScale();
            HideNonSelectedCards(dealCardView);
            _presenter.OnCardSelected(data);
        }

        private void HideNonSelectedCards(DealCardView dealCardView)
        {
            foreach (Transform child in _itemContent)
            {
                if (child.TryGetComponent(out DealCardView view))
                    view.gameObject.SetActive(dealCardView == view);
            }
        }

        private void OnCardMouseEnter(DealCardView view) => view.ScaleUp();

        private void OnCardMouseExit(DealCardView view) => view.RestoreScale();

        public void ClearCards()
        {
            _bg.gameObject.SetActive(false);
            _itemContent.gameObject.SetActive(false);
            foreach (Transform child in _itemContent)
            {
                if (child.TryGetComponent(out DealCardView view))
                {
                    view.RestoreScale();
                    view.gameObject.SetActive(false);
                    view.OnClick -= OnCardClicked;
                    view.OnMouseEnter -= OnCardMouseEnter;
                    view.OnMouseExit -= OnCardMouseExit;
                }
            }
        }

        private void OnDestroy() => _startButton.onClick.RemoveListener(OnStartClicked);
    }
}