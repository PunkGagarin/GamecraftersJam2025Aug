using System;
using Jam.Scripts.Gameplay.Rooms.Events;
using Jam.Scripts.Gameplay.Rooms.Events.Data;

namespace Jam.Scripts.Gameplay.Rooms
{
    public class RoomRewardBus
    {
        public event Action OnRoomCompleted = delegate { };
        public void InvokeRoomCompleted() => OnRoomCompleted.Invoke();
    }
}