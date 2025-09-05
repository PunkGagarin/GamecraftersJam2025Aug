namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public sealed class EffectInstance
    {
        public EffectInstance(TargetType Targeting, IEffectPayload Payload)
        {
            this.Targeting = Targeting;
            this.Payload = Payload;
        }

        public TargetType Targeting { get; }
        public IEffectPayload Payload { get; }

        public void Deconstruct(out TargetType Targeting, out IEffectPayload Payload)
        {
            Targeting = this.Targeting;
            Payload = this.Payload;
        }
    }
}