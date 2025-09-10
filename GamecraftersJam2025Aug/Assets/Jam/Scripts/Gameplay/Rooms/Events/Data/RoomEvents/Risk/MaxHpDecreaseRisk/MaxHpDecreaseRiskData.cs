using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class MaxHpDecreaseRiskData : RoomRiskEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; }
        [field: SerializeField] public RoomEventRiskType Type { get; set; } = RoomEventRiskType.MaxHpDecrease;
        [field: SerializeField] public float Value { get; set; }

        public override RiskInstance ToInstance() => new(Type, new MaxHpDecreaseRiskPayload(Value));
    }
}