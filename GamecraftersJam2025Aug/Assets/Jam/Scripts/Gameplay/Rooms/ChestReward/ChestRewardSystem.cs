using System;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.ChestReward
{
    public class ChestRewardSystem : IInitializable, IDisposable
    {
        [Inject] private ChestRewardView _chestPrefab;
        [Inject] private RoomRewardBus _roomRewardBus;
        [Inject] private BattleWinGenerator _winGenerator;

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
            WinDto winData = _winGenerator.GenerateWinData();
            _roomRewardBus.InvokeChestOpened(winData);
            _chestPrefab.Hide();
        }

        public void Dispose()
        {
            _chestPrefab.OnClicked -= OnChestOpened;
        }
    }
}