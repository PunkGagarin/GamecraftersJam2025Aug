using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.DamageRisk
{
    public class DamageRiskData : RoomRiskEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; set; }
        [field: SerializeField] public RoomEventRiskType Type { get; set; } = RoomEventRiskType.Damage;
        [field: SerializeField] public float Value { get; set; }

        public override RiskInstance ToInstance() => new(Type, new DamageRiskPayload(Value));
    }
}