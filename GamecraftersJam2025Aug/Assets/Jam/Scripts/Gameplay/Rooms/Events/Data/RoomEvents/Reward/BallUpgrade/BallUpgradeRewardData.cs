using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public class BallUpgradeRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public RoomEventRewardType Type { get; set; } = RoomEventRewardType.BallUpgrade;

        public override RewardInstance ToInstance() => new(Type, new BallUpgradePayload());
    }
}