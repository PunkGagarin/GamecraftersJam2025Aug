using System;
using Jam.Scripts.Gameplay.Rooms.Events.Data;

namespace Jam.Scripts.Gameplay.Rooms
{
    public class RoomRewardBus
    {
        public event Action OnRoomCompleted = delegate { };
        public event Action<RoomEvent> OnEventReward = delegate { };
        public void InvokeRoomCompleted() => OnRoomCompleted.Invoke();
        public void InvokeEventReward(RoomEvent roomEvent) => OnEventReward.Invoke(roomEvent);
    }
}