using System;
using Jam.Scripts.Gameplay.Rooms;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.ChestReward
{
    public class ChestRewardSystem : IInitializable, IDisposable
    {
        [Inject] private ChestRewardView _chestPrefab;
        [Inject] private RoomEventBus _roomEventBus;

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
            _roomEventBus.InvokeRoomCompleted();
            _chestPrefab.Hide();
        }

        public void Dispose()
        {
            _chestPrefab.OnClicked -= OnChestOpened;
        }
    }
}