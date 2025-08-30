using System;
using Jam.Scripts.MapFeature.Map.Data;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapEventBus
    {
        public Action<MapModel> OnMapCreated = delegate { };
        public Action<Room> OnRoomChosen = delegate { };
    }
}