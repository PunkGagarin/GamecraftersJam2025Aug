using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public abstract class RoomRiskEventData
    {
        public abstract Sprite Sprite { get; }
        public abstract RiskInstance ToInstance();
    }
}