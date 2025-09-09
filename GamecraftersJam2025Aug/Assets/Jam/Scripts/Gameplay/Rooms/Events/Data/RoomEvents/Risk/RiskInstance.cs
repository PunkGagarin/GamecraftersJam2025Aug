namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public sealed class RiskInstance
    {
        public RiskInstance(RoomEventRiskType type, IRiskPayload payload)
        {
            Type = type;
            Payload = payload;
        }

        public IRiskPayload Payload { get; private set; }
        public RoomEventRiskType Type { get; private set; }
    }
}