using System;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomDealEventPresenter : IInitializable, IDisposable
    {
        [Inject] private RoomDealEventView _view;
        [Inject] private RoomEventBus _roomEventBus;

        public void Initialize()
        {
            _roomEventBus.OnStartDealEvent += OnStartDealEvent;
        }

        private void OnStartDealEvent(DealUiData data)
        {
            _view.ShowDealEvent(data);
        }

        public void Dispose()
        {
            _roomEventBus.OnStartDealEvent -= OnStartDealEvent;
        }
    }
}
