using System;

namespace Jam.Scripts.Gameplay.Rooms
{
    public class RoomEventBus
    {
        public event Action OnRoomCompleted = delegate { };
        public void InvokeRoomCompleted() => OnRoomCompleted.Invoke();
    }
}