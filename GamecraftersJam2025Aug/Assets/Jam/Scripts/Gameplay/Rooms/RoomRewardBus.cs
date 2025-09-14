using System;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;

namespace Jam.Scripts.Gameplay.Rooms
{
    public class RoomRewardBus
    {
        public event Action OnRoomCompleted = delegate { };
        public event Action<WinDto> OnChestOpened = delegate { };
        
        public void InvokeRoomCompleted() => OnRoomCompleted.Invoke();
        public void InvokeChestOpened(WinDto winData) => OnChestOpened.Invoke(winData);
    }
}