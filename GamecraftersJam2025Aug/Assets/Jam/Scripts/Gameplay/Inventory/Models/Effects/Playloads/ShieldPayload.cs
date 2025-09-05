namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public sealed class ShieldPayload : IEffectPayload
    {
        public ShieldPayload(int Amount)
        {
            this.Amount = Amount;
        }

        public int Amount { get; }

        public void Deconstruct(out int Amount)
        {
            Amount = this.Amount;
        }
    }
}