using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.GoldRisk
{
    public class GoldRiskData : RoomRiskEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public RoomEventRiskType Type { get; set; } = RoomEventRiskType.Gold;
        [field: SerializeField] public float Value { get; set; }

        public override RiskInstance ToInstance() => new(Type, new GoldRiskPayload(Value));
    }
}