namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class HealRewardPayload : IRewardPayload
    {
        public float HealPercent { get; set;  }
        public HealRewardPayload(float healPercent) => HealPercent = healPercent;
    }
}