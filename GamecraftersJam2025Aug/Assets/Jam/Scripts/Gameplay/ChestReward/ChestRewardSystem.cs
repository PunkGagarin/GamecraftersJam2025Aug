using System;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.ChestReward
{
    public class ChestRewardSystem : IInitializable, IDisposable
    {
        [Inject] private ChestRewardView _chestPrefab;

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
            //todo who to inform??
            _chestPrefab.Hide();
        }

        public void Dispose()
        {
            _chestPrefab.OnClicked -= OnChestOpened;
        }
    }
}