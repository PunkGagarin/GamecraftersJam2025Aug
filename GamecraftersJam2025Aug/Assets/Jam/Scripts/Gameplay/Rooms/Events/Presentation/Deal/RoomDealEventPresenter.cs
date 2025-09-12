using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomDealEventPresenter : IInitializable, IDisposable
    {
        [Inject] private RoomDealEventView _view;
        [Inject] private DealCardView _dealCardPrefab;
        [Inject] private RoomEventBus _roomEventBus;
        [Inject] private RoomEventService _roomEventService;

        private DealUiData _data;

        public void Initialize() => 
            _roomEventBus.OnStartDealEvent += OnStartDealEvent;

        private void OnStartDealEvent(DealUiData data) => 
            _view.ShowDealEvent(data);

        public void OnStartClicked()
        {
            List<KeyValuePair<DealCardView, DealButtonData>> prefabs = new();
            foreach (var dealButtonData in _data.Buttons)
            {
                var keyValuePair = new KeyValuePair<DealCardView, DealButtonData>(_dealCardPrefab, dealButtonData);
                prefabs.Add(keyValuePair);
            }

            _view.InitializePrefabs(prefabs);
        }

        public void OnCardSelected(DealButtonData data)
        {
            _roomEventService.ProcessReward(data.Reward);   
            _roomEventService.ProcessRisk(data.Risk);   
        }

        public void Dispose() => 
            _roomEventBus.OnStartDealEvent -= OnStartDealEvent;
    }
}
