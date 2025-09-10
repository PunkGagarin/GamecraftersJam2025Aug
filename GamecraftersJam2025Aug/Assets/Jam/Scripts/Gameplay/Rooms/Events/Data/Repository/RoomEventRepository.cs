using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [CreateAssetMenu(fileName = "RoomEventRepository", menuName = "Game Resources/RoomEvents/RoomEventRepository")]
    public class RoomEventRepository : ScriptableObject
    {
        [SerializeField] public List<RoomDealEvent> RoomDealEvents;
        [SerializeField] public List<RoomFightEvent> RoomFightEvents;
        [SerializeField] public List<RoomRewardEvent> RoomRewardEvents;
    }
}