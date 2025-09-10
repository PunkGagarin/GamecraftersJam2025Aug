using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [CreateAssetMenu(fileName = "RoomFightEvent", menuName = "Game Resources/RoomEvents/RoomFightEvent")]
    public class RoomFightEvent : RoomEvent
    {
        [field: SerializeField] public RoomEventType Type { get; private set; } = RoomEventType.Fight;
    }
}