using System;
using Jam.Prefabs.Gameplay.Gold;
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
        [Inject] private BattleEventBus _roomRewardBus;
        [Inject] private BattleWinUi _winUi;
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private WinRewardSystem _rewardSystem;

        public void Initialize()
        {
            _roomRewardBus.OnWin += ShowRoomCompletedScreen;
            _winUi.ToMapButton.onClick.AddListener(OpenMap);
            _winUi.UpgradeButton.onClick.AddListener(OpenUpgrade);
            _winUi.HealButton.HealButton.onClick.AddListener(Heal);
            
            var ballViews = _winUi.BallBuyViews;
            foreach (BallRewardWithGoldView ballView in ballViews)
            {
                ballView.OnClick += TryToBuyBall;
            }
        }

        private void TryToBuyBall(RewardCardView arg1, ICardUiData arg2)
        {
            
        }

        public void Dispose()
        {
            _roomRewardBus.OnWin -= ShowRoomCompletedScreen;
            _winUi.ToMapButton.onClick.RemoveListener(OpenMap);
            _winUi.UpgradeButton.onClick.RemoveListener(OpenUpgrade);
            _winUi.HealButton.HealButton.onClick.RemoveListener(Heal);
        }

        private void Heal()
        {
            _rewardSystem.Heal();
            _winUi.HealButton.HealButton.interactable = false;
        }

        private void OpenUpgrade()
        {
            Debug.LogError("open upgrade not implemented");
        }

        private void ShowRoomCompletedScreen(WinDto winDto)
        {
            //todo: results
            _winUi.Show();
            _winUi.InitWinData(winDto);
            _winUi.SetEnoughGoldForHeal(_rewardSystem.HasGoldForHeal());
            _winUi.SetEnoughGoldForUpgrade(_rewardSystem.HasGoldForUpgrade());
            _winUi.SetEnoughGoldToBuyBall(_rewardSystem.HasGoldToBuyBall());
        }


        private void OpenMap()
        {
            _winUi.Hide();
            _mapEventBus.RoomCompleted();
        }
    }
}