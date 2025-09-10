using System;
using Jam.Scripts.Gameplay.Rooms.Events;

namespace Jam.Scripts.Gameplay.Rooms
{
    public class RoomRewardBus
    {
        public event Action OnRoomCompleted = delegate { };
        public void InvokeRoomCompleted() => OnRoomCompleted.Invoke();
    }
}