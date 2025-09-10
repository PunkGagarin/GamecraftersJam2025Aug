using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class BallRiskData : RoomRiskEventData
    {
        [field: SerializeField] public override Sprite Sprite { get; }
        [field: SerializeField] public RoomEventRiskType Type { get; set; } = RoomEventRiskType.LoseBall;
        [field: SerializeField] public BallSo Ball { get; set; }

        public override RiskInstance ToInstance() => new(Type, new BallRiskPayload(Ball));
    }
}