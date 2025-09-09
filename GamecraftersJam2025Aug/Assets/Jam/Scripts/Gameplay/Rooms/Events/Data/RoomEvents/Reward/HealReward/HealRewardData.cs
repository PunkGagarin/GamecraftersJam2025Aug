using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public class HealRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; }
        [field: SerializeField] public RoomEventRewardType Type { get; set; } = RoomEventRewardType.HealPlayer;
        [field: SerializeField] public float HealPercent { get; set; }

        public override RewardInstance ToInstance() => new(Type, new HealRewardPayload(HealPercent));
    }
}