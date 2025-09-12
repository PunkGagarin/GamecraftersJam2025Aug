namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class MaxHpDecreaseRiskPayload : IRiskPayload
    {
        public float Value { get; set; }
        public MaxHpDecreaseRiskPayload(float value) => Value = value;
    }
}