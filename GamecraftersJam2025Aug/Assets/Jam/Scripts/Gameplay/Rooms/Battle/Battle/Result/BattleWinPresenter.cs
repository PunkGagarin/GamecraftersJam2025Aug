using System;
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
        [Inject] private BattleWinUi _winUi;
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private WinRewardSystem _rewardSystem;
        [Inject] private PlayerInventoryPresenter _inventoryPresenter;

        public void Initialize()
        {
            _battleEventBus.OnWin += ShowRoomCompletedScreen;
            _roomRewardBus.OnChestOpened += ShowRoomCompletedScreen;
            _winUi.ToMapButton.onClick.AddListener(OpenMap);
            _winUi.UpgradeButton.onClick.AddListener(OpenUpgrade);
            _winUi.HealButton.HealButton.onClick.AddListener(Heal);

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
        }

        private void TryToBuyBall(RewardCardView view, ICardUiData ballData)
        {
            BallRewardCardUiData data = ballData as BallRewardCardUiData;
            BallRewardWithGoldView castedView = (BallRewardWithGoldView)view;

            if (!castedView.IsInteractable() || data == null || castedView == null) return;

            _rewardSystem.TryToBuyBall(data.Type, data.Grade, data.GoldPrice);
            castedView.SetInteractable(false);
            SetGoldStatus();
        }

        private void Heal()
        {
            _rewardSystem.Heal();
            _winUi.HealButton.HealButton.interactable = false;
        }

        private void OpenUpgrade()
        {
            _inventoryPresenter.OpenUpgrade();
        }

        private void ShowRoomCompletedScreen(WinDto winDto)
        {
            _winUi.Show();
            _winUi.InitWinData(winDto);
            SetGoldStatus();
        }

        private void SetGoldStatus()
        {
            _winUi.SetEnoughGoldForHeal(_rewardSystem.HasGoldForHeal());
            _winUi.SetEnoughGoldForUpgrade(_rewardSystem.HasGoldForUpgrade());
            _winUi.SetEnoughGoldToBuyBalls(_rewardSystem.HasGoldToBuyFirstGrade(),
                _rewardSystem.HasGoldToBuySecondGrade());
        }


        private void OpenMap()
        {
            _winUi.Hide();
            _mapEventBus.RoomCompleted();
        }
    }
}