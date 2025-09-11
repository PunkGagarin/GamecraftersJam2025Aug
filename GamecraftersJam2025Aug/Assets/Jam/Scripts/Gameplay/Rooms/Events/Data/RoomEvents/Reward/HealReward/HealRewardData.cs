using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public class HealRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public RoomEventRewardType Type { get; set; } = RoomEventRewardType.HealPlayer;
        [field: SerializeField] public float Value { get; set; }

        public override RewardInstance ToInstance() => new(Type, new HealRewardPayload(Value));
    }
}