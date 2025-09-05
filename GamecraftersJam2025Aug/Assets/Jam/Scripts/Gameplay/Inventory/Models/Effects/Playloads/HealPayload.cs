namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public sealed class HealPayload : IEffectPayload
    {
        public HealPayload(int amount)
        {
            this.Amount = amount;
        }

        public int Amount { get; }
    }
}