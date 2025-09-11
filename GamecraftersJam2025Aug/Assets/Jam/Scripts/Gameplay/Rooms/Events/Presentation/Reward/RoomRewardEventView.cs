using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomRewardEventView : ContentUi
    {
        [Inject] private RoomRewardEventPresenter _presenter;

        [SerializeField] private RectTransform _itemsContent;
        [SerializeField] private Image _bg;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _getRewardButton;
        
        private void Start()
        {
            _startButton.onClick.AddListener(OnStartClicked);
            _getRewardButton.onClick.AddListener(OnGetRewardClicked);
            _startButton.gameObject.SetActive(false);
            _getRewardButton.gameObject.SetActive(false);
            _itemsContent.gameObject.SetActive(false);
            _bg.gameObject.SetActive(false);
        }

        private void OnGetRewardClicked() => _presenter.OnGetRewardClicked();

        public void SetGetRewardButtonEnable(bool isInteractable) => _getRewardButton.interactable = isInteractable;
        private void OnRandomBallSelected(RandomBallRewardCardUiData data, BallRewardCardUiData selectedBallData) => 
            _presenter.OnRandomBallSelected(data, selectedBallData.Type, selectedBallData.Grade);

        public void InitializePrefabs(List<KeyValuePair<RewardView, IRewardCardUiData>> prefabs)
        {
            foreach (var rewardCardUiData in prefabs)
            {
                var view = Instantiate(rewardCardUiData.Key, _itemsContent);
                view.SetData(rewardCardUiData.Value);
                switch (view)
                {
                    case RandomRewardView randomRewardView:
                        randomRewardView.OnRandomBallSelected += OnRandomBallSelected;
                        break;
                    case BallUpgradeRewardView ballUpgradeRewardView:
                        ballUpgradeRewardView.OpenBallUpgradePopup += OnBallUpgradeClicked;
                        break;
                }
            }
        }

        private void OnBallUpgradeClicked()
        {
            // todo uhm idk??
            _presenter.OnBallUpgraded();
        }

        private void OnStartClicked()
        {
            _startButton.gameObject.SetActive(false);
            _getRewardButton.gameObject.SetActive(true);
            _itemsContent.gameObject.SetActive(true);
            _bg.gameObject.SetActive(true);
            _presenter.OnStartClicked();
        }

        public void ShowRewardEvent(RewardUiData data)
        {
            _startButton.image.sprite = data.Icon;
            _startButton.gameObject.SetActive(true);
        }

        public void ClearRewards()
        {
            _getRewardButton.gameObject.SetActive(false);
            _itemsContent.gameObject.SetActive(false);
            _bg.gameObject.SetActive(false);
            foreach (Transform child in _itemsContent) 
                Destroy(child.gameObject);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
            _getRewardButton.onClick.RemoveListener(OnGetRewardClicked);
            foreach (Transform child in _itemsContent)
            {
                if (child.TryGetComponent(out RandomRewardView randomRewardView)) 
                    randomRewardView.OnRandomBallSelected -= OnRandomBallSelected;
                if (child.TryGetComponent(out BallUpgradeRewardView ballUpgradeRewardView))
                    ballUpgradeRewardView.OpenBallUpgradePopup -= OnBallUpgradeClicked;
            }
        }
    }
}