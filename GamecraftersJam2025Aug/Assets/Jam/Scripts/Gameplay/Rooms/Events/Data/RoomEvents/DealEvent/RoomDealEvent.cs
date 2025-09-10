using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Events.DamageRisk;
using Jam.Scripts.Gameplay.Rooms.Events.GoldRisk;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [CreateAssetMenu(fileName = "RoomDealEvent", menuName = "Game Resources/RoomEvents/RoomDealEvent")]
    public class RoomDealEvent : RoomEvent
    {
        [field: SerializeField] public RoomEventType Type { get; private set; } = RoomEventType.Deal;
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public List<RoomDealData> DealData { get; private set; } = new();
        
    }
}