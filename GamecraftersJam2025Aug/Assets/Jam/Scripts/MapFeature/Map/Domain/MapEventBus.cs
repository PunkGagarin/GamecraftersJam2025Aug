using System;
using Jam.Scripts.MapFeature.Map.Data;

namespace Jam.Scripts.MapFeature.Map.Domain
{
    public class MapEventBus
    {
        public event Action<MapModel> OnMapCreated = delegate { };
        public event Action<Room> OnRoomChosen = delegate { };
        public event Action OnRoomCompleted = delegate { };
        public event Action<Room> OnOpenMap = delegate { };

        public virtual void MapInitialized(MapModel model) => OnMapCreated.Invoke(model);
        public virtual void RoomChosen(Room room) => OnRoomChosen.Invoke(room);
        public virtual void RoomCompleted() => OnRoomCompleted.Invoke();
        public virtual void OpenMap(Room room) => OnOpenMap.Invoke(room);
    }
}