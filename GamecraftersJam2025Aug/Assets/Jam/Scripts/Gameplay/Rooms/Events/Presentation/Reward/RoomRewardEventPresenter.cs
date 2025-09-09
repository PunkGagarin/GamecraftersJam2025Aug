using System;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomRewardEventPresenter : IInitializable, IDisposable
    {
        [Inject] private RoomDealEventView _view;
        [Inject] private RoomEventBus _roomEventBus;

        public void Initialize()
        {
            _roomEventBus.OnStartRewardEvent += OnStartRewardEvent;
        }

        private void OnStartRewardEvent(RewardUiData data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _roomEventBus.OnStartRewardEvent -= OnStartRewardEvent;
        }
    }
}