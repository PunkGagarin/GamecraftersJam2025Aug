using System;
using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    [Serializable]
    public class ConcreteBallRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField]
        public RoomEventRewardType Type { get; set; } = RoomEventRewardType.ConcreteBall;
        [field: SerializeField]
        public BallSo ConcreteBall { get; set; }

        public override RewardInstance ToInstance() => new(Type, new ConcreteBallRewardPayload(ConcreteBall));
        
    }
}