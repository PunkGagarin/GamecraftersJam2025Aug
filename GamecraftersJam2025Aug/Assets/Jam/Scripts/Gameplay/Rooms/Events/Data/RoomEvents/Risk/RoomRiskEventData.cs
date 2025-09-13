using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public abstract class RoomRiskEventData
    {
        public abstract Sprite Sprite { get; set; }
        public abstract RiskInstance ToInstance();
    }
}