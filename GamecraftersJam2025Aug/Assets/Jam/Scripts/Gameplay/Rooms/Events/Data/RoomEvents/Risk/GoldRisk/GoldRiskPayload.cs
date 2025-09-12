namespace Jam.Scripts.Gameplay.Rooms.Events.GoldRisk
{
    public class GoldRiskPayload : IRiskPayload
    {
        public float Value { get; set; }
        public GoldRiskPayload(float value) => Value = value;
    }
}