namespace Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward
{
    public class MaxHpIncreaseRewardPayload : IRewardPayload
    {
        public float Value { get; set; }
        public MaxHpIncreaseRewardPayload(float value) => Value = value;
    }
}