using System;
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

        private void OnStartDealEvent(DealUiData data)
        {
            _data = data;
            _view.Show();
            _view.ShowDealEvent(data);
        }

        public void OnStartClicked() => _view.Initialize(_data.Buttons);

        public void OnCardSelected(DealButtonData data)
        {
            _view.Hide();
            _roomEventService.ProcessReward(data.Reward);   
            _roomEventService.ProcessRisk(data.Risk);   
            _roomEventBus.EventFinished();
        }

        public void Dispose() => 
            _roomEventBus.OnStartDealEvent -= OnStartDealEvent;
    }
}
