using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public class GoldRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; }
        [field: SerializeField] public RoomEventRewardType Type { get; set; } = RoomEventRewardType.Gold;

        [field: SerializeField] public float Amount { get; set; }

        public override RewardInstance ToInstance() => new(Type, new GoldRewardPayload(Amount));
    }
}