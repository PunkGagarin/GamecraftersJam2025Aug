using System;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.MapFeature.Map.Domain;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class RewardPresenter : IInitializable, IDisposable
    {
        [Inject] private RoomRewardBus _roomRewardBus;
        [Inject] private BattleRewardUi _rewardUi;
        [Inject] private MapEventBus _mapEventBus;

        public void Initialize()
        {
            _roomRewardBus.OnRoomCompleted += ShowRoomCompletedScreen;
            _rewardUi.ToMapButton.onClick.AddListener(OpenMap);
        }

        public void Dispose()
        {
            _roomRewardBus.OnRoomCompleted -= ShowRoomCompletedScreen;
            _rewardUi.ToMapButton.onClick.RemoveListener(OpenMap);
        }

        private void ShowRoomCompletedScreen()
        {
            //todo: results
            _rewardUi.Show();
        }


        private void OpenMap()
        {
            _rewardUi.Hide();
            _mapEventBus.RoomCompleted();
        }
    }
}