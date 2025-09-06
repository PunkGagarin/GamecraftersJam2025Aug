using System;
using Jam.Scripts.Gameplay.Rooms.Events.Data;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomEventPresenter : IInitializable, IDisposable
    {
        [Inject] private RoomEventView _view;
        [Inject] private RoomEventBus _roomEventBus;
        
        public void Initialize()
        {
            _roomEventBus.OnStartEvent += OnStartEvent;
        }

        private void OnStartEvent(RoomEvent roomEvent)
        {
            _view.ShowEvent(roomEvent);
;        }

        public void Dispose()
        {
            _roomEventBus.OnStartEvent -= OnStartEvent;
        }
    }
}