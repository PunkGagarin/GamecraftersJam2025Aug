using System;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.MapFeature.Map.Presentation;
using Zenject;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapPresenter : IInitializable, IDisposable
    {
        [Inject] private MapView _mapView;
        [Inject] private MapService _mapService;
        [Inject] private MapEventBus _mapEventBus;

        public void Initialize()
        {
            _mapEventBus.OnMapCreated += OnMapInitialize;
            _mapEventBus.OnRoomChosen += SetCurrentRoom;
        }

        private void SetCurrentRoom(Room targetRoom)
        {
            _mapView.SetCurrentRoom(targetRoom);
        }

        public void OnRoomNodeClicked(Room targetRoom) =>
            _mapService.OnRoomNodeClicked(targetRoom);

        private void OnMapInitialize(MapModel mapModel)
        {
            _mapView.ShowMap(mapModel.Floors, mapModel.MiddleRoomIndex, mapModel.CurrentRoom);
        }


        public void Dispose()
        {
            _mapEventBus.OnMapCreated -= OnMapInitialize;
            _mapEventBus.OnRoomChosen -= SetCurrentRoom;
        }
    }
}