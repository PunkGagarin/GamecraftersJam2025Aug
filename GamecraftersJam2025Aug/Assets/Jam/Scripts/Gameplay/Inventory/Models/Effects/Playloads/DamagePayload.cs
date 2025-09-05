namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public sealed class DamagePayload : IEffectPayload
    {
        public DamagePayload(int damage)
        {
            this.Damage = damage;
        }

        public int Damage { get; }
    }
}