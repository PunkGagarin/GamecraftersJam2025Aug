using System;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapService : IInitializable, IDisposable
    {
        [Inject] private MapFactory _mapFactory;
        [Inject] private MapEventBus _mapEventBus;

        private MapModel _mapModel;

        public void Initialize()
        {
            _mapEventBus.OnRoomCompleted += OnRoomCompleted;
        }

        private void OnRoomCompleted()
        {
            if(_mapModel == null) CreateModel();
            var curRoom = _mapModel?.CurrentRoom;
            _mapEventBus.OpenMap(curRoom);
        }

        private void CreateModel()
        {
            var model = _mapFactory.CreateMap();
            _mapModel = model;
            _mapEventBus.MapInitialized(_mapModel);
        }

        public void OnRoomNodeClicked(Room targetRoom)
        {
            _mapModel.CurrentRoom = targetRoom;
            _mapEventBus.RoomChosen(targetRoom);
        }


        public void Dispose()
        {
            _mapEventBus.OnRoomCompleted -= OnRoomCompleted;
        }

    }
}