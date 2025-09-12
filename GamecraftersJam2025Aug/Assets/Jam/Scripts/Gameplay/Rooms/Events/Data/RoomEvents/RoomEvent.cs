using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class RoomEvent : ScriptableObject
    {
        [SerializeField] public RoomEventKeyType Id;
        [SerializeField] public List<string> ClownMonologueKeys;
        [SerializeField] public int Weight;
    }
}