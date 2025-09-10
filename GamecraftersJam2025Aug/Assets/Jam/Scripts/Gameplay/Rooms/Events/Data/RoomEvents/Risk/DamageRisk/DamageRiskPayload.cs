namespace Jam.Scripts.Gameplay.Rooms.Events.DamageRisk
{
    public class DamageRiskPayload : IRiskPayload
    {
        public float Value { get; set; }
        public DamageRiskPayload(float value) => Value = value;
    }
}