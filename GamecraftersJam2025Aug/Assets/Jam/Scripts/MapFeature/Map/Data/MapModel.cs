using System;
using System.Collections.Generic;

namespace Jam.Scripts.MapFeature.Map.Data
{
    public class MapModel
    {
        public event Action<MapModel> OnCurrentRoomChanged = delegate { };

        public List<Floor> Floors { get; set; }
        public Room CurrentRoom { get; private set; }

        public void SetCurrentRoom(Room room)
        {
            CurrentRoom = room;
            OnCurrentRoomChanged.Invoke(this);
        }
    }
}