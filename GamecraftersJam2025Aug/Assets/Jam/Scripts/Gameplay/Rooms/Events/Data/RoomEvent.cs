using System.Collections.Generic;
using Jam.Scripts.GameplayData.Definitions;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Data
{
    [CreateAssetMenu(fileName = "RoomEvent", menuName = "Game Resources/RoomEvents/RoomEvent")]
    public class RoomEvent: Definition
    {
        [SerializeField] public int Id;
        [SerializeField] public string Name;
        [SerializeField] public RoomEventType Type;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public float Weight;
        [SerializeField] public List<string> ClownMonologueIds;
        
        //todo reward & risk
    }
}