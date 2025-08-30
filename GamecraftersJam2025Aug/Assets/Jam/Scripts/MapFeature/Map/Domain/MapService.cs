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

        public void Initialize() => 
            CreateModel();

        private void CreateModel()
        {
            var model = _mapFactory.CreateMap();
            _mapModel = model;
            _mapEventBus.OnMapCreated.Invoke(_mapModel);
        }

        public void OnRoomNodeClicked(Room targetRoom) => 
            _mapEventBus.OnRoomChosen(targetRoom);


        public void Dispose()
        {
        }

    }
}