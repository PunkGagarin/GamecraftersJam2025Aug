using System;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
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
        }


        private void OpenMap()
        {
            _winUi.Hide();
            _mapEventBus.RoomCompleted();
        }
    }
}