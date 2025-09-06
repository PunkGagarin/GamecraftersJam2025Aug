using System;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.ChestReward
{
    public class ChestRewardSystem : IInitializable, IDisposable
    {
        [Inject] private ChestRewardView _chestPrefab;
        [Inject] private RoomRewardBus _roomRewardBus;

        public void Initialize()
        {
            _chestPrefab.OnClicked += OnChestOpened;
        }

        public void Handle(Room room)
        {
            _chestPrefab.Show();
        }

        private void OnChestOpened(ChestRewardView view)
        {
            _roomRewardBus.InvokeRoomCompleted();
            _chestPrefab.Hide();
        }

        public void Dispose()
        {
            _chestPrefab.OnClicked -= OnChestOpened;
        }
    }
}