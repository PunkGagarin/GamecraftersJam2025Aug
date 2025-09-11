using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward
{
    public class MaxHpIncreaseRewardData : RoomRewardEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField]
        public RoomEventRewardType Type { get; set; } = RoomEventRewardType.MaxHpIncrease;
        [field: SerializeField]
        public float Value { get; set; }

        public override RewardInstance ToInstance() => new(Type, new MaxHpIncreaseRewardPayload(Value));

    }
}