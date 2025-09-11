using System;
using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public class BallUpgradeRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public RoomEventRewardType Type { get; set; } = RoomEventRewardType.BallUpgrade;
        [field: SerializeField] public BallType BallType { get; set; }

        public override RewardInstance ToInstance() => new(Type, new BallUpgradePayload(BallType));
    }
}