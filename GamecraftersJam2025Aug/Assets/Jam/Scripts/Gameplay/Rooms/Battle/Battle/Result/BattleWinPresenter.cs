using System;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation.WithGold;
using Jam.Scripts.MapFeature.Map.Domain;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class BattleWinPresenter : IInitializable, IDisposable
    {
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private RoomRewardBus _roomRewardBus;
        [Inject] private InventoryBus _inventoryBus;
        [Inject] private BattleWinUi _winUi;
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private WinRewardSystem _rewardSystem;
        [Inject] private PlayerInventoryPresenter _inventoryPresenter;
        [Inject] private readonly AudioService _audioService;
        
        public void Initialize()
        {
            _battleEventBus.OnWin += ShowRoomCompletedScreen;
            _roomRewardBus.OnChestOpened += ShowRoomCompletedScreen;
            _winUi.ToMapButton.onClick.AddListener(OpenMap);
            _winUi.UpgradeButton.onClick.AddListener(OpenUpgrade);
            _winUi.HealButton.HealButton.onClick.AddListener(Heal);
            _inventoryBus.OnBallUpgraded += SetGoldVisualStatus;

            var ballViews = _winUi.BallBuyViews;
            foreach (BallRewardWithGoldView ballView in ballViews)
            {
                ballView.OnClick += TryToBuyBall;
            }
        }

        public void Dispose()
        {
            _battleEventBus.OnWin -= ShowRoomCompletedScreen;
            _roomRewardBus.OnChestOpened -= ShowRoomCompletedScreen;
            _winUi.ToMapButton.onClick.RemoveListener(OpenMap);
            _winUi.UpgradeButton.onClick.RemoveListener(OpenUpgrade);
            _winUi.HealButton.HealButton.onClick.RemoveListener(Heal);
            _inventoryBus.OnBallUpgraded -= SetGoldVisualStatus;
        }

        private void TryToBuyBall(RewardCardView view, ICardUiData ballData)
        {
            BallRewardCardUiData data = ballData as BallRewardCardUiData;
            BallRewardWithGoldView castedView = (BallRewardWithGoldView)view;
            
            if (data == null || castedView == null)
            {
                _audioService.PlaySound(Sounds.error.ToString());    
                return;
            }

            if (!(castedView.IsFirstGrade() && _rewardSystem.HasGoldToBuyFirstGrade()))
            {
                _audioService.PlaySound(Sounds.error.ToString());
                castedView.TryGetComponent<RectTransform>(out var rectTransform);
                if (rectTransform != null) ShowErrorToRectTransform(rectTransform);
                return;
            }

            if (!_rewardSystem.HasGoldToBuySecondGrade())
            {
                _audioService.PlaySound(Sounds.error.ToString());
                castedView.TryGetComponent<RectTransform>(out var rectTransform);
                if (rectTransform != null) ShowErrorToRectTransform(rectTransform);
                return;
            }
            
            _audioService.PlaySound(Sounds.getGold.ToString());
            _rewardSystem.TryToBuyBall(data.Type, data.Grade, data.GoldPrice);
            castedView.SetInteractable(false);
            castedView.SetIsBought(true);
            SetGoldVisualStatus();
        }

        private void Heal()
        {
            if (_rewardSystem.HasGoldForHeal())
            {
                _audioService.PlaySound(Sounds.buttonClick.ToString());
                _rewardSystem.Heal();
                _winUi.HealButton.HealButton.interactable = false;
                SetGoldVisualStatus();
            }
            else
            {
                _audioService.PlaySound(Sounds.error.ToString());
                ShowErrorToRectTransform(_winUi.HealButton.HealButton.GetComponent<RectTransform>());
            }
        }

        private void ShowErrorToRectTransform(RectTransform rt)
        {
            rt.DOKill();
            rt.DOShakeAnchorPos(0.5f, strength: new Vector2(10, 0), vibrato: 20, randomness: 90, snapping: false,
                fadeOut: true);
        }

        private void OpenUpgrade()
        {
            if (_rewardSystem.HasGoldForUpgrade())
            {
                _audioService.PlaySound(Sounds.buttonClick.ToString());
                _inventoryPresenter.OpenUpgrade();
            }
            else
            {
                _audioService.PlaySound(Sounds.error.ToString());
                ShowErrorToRectTransform(_winUi.UpgradeButton.GetComponent<RectTransform>());
            }
        }

        private void ShowRoomCompletedScreen(WinDto winDto)
        {
            _winUi.Show();
            _winUi.InitWinData(winDto);
            SetBoughtToFalse();
            SetGoldVisualStatus();
        }

        private void SetBoughtToFalse()
        {
            foreach (var ballBuyView in _winUi.BallBuyViews)
                ballBuyView.SetIsBought(false);
        }

        /// <summary>
        /// Отображение визуального статуса возможности нажзать на кнопку
        /// </summary>
        private void SetGoldVisualStatus()
        {
            _winUi.SetEnoughGoldForHeal(_rewardSystem.HasGoldForHeal());
            _winUi.SetEnoughGoldForUpgrade(_rewardSystem.HasGoldForUpgrade());
            _winUi.SetEnoughGoldToBuyBalls(
                _rewardSystem.HasGoldToBuyFirstGrade(),
                _rewardSystem.HasGoldToBuySecondGrade()
            );
        }


        private void OpenMap()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _winUi.Hide();
            _mapEventBus.RoomCompleted();
        }
    }
}