namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class GoldRewardPayload : IRewardPayload
    {
        public float Amount { get; set;  }
        public GoldRewardPayload(float goldAmount) => Amount = goldAmount;
    }
}