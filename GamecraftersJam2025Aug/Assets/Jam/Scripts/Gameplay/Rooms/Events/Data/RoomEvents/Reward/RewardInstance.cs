namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public sealed class RewardInstance
    {
        public RewardInstance(RoomEventRewardType type, IRewardPayload payload)
        {
            Type = type;
            Payload = payload;
        }

        public IRewardPayload Payload { get; private set; }
        public RoomEventRewardType Type { get; private set; }
    }
}