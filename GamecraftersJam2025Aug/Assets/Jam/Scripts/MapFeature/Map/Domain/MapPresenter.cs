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

        private void SetCurrentRoom(MapModel mapModel, int nextFloorId, Room room, Room newRoom)
        {
            //todo 
            Floor prevFloor = null;
            Floor nextFloor = null;
            if (nextFloorId - 2 ! < 0)
            {
                prevFloor = mapModel.Floors[nextFloorId - 2];
            }

            if (nextFloorId + 1 ! > mapModel.Floors.Count)
            {
                nextFloor = mapModel.Floors[nextFloorId + 1];
            }

            _mapView.ShowCurrentRoom(room, prevFloor, nextFloor);
            _mapView.AnimateConnection(room, newRoom);
        }

        public void OnRoomNodeClicked(Room targetRoom)
        {
            _mapService.OnRoomNodeClicked(targetRoom);
        }

        private void OnMapInitialize(MapModel mapModel)
        {
            _mapView.ShowMap(mapModel.Floors, mapModel.MiddleRoomIndex);
        }


        public void Dispose()
        {
            _mapEventBus.OnMapCreated -= OnMapInitialize;
            _mapEventBus.OnRoomChosen -= SetCurrentRoom;
        }
    }
}