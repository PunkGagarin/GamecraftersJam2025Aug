using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.BallRisk
{
    public class BallRiskData : RoomRiskEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; }
        [field: SerializeField] public RoomEventRiskType Type { get; set; } = RoomEventRiskType.LoseBall;

        public override RiskInstance ToInstance() => new(Type, new BallRiskPayload());
    }
}