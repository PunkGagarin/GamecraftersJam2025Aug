namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public sealed class PoisonPayload : IEffectPayload
    {
        public PoisonPayload(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; }
    }
}