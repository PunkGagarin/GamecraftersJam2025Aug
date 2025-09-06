using System;
using Jam.Scripts.Gameplay.Rooms.Events.Data;

namespace Jam.Scripts.Gameplay.Rooms.Events.Domain
{
    public class RoomEventBus
    {
        public event Action<RoomEvent> OnStartEvent = delegate { };

        public void StartEvent(RoomEvent roomEvent) => OnStartEvent.Invoke(roomEvent);
    }
}