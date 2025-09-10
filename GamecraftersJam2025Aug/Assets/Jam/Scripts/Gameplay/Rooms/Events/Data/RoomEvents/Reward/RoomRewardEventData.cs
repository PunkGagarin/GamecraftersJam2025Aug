using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public abstract class RoomRewardEventData
    {
        public abstract Sprite Sprite { get; }
        public abstract RewardInstance ToInstance();
    }
}