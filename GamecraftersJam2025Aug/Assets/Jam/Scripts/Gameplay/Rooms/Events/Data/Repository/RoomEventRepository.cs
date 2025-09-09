using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [CreateAssetMenu(fileName = "RoomEventRepository", menuName = "Game Resources/RoomEvents/RoomEventRepository")]
    public class RoomEventRepository : ScriptableObject
    {
        public List<RoomDealEvent> RoomDealEvents;
        public List<RoomFightEvent> RoomFightEvents;
        public List<RoomRewardEvent> RoomRewardEvents;
    }
}