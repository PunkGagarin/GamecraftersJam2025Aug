namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class CriticalDamagePayload : IEffectPayload
    {
        public int Damage { get; }
        public int Chance { get; }
        public float Multiplier { get; }

        public CriticalDamagePayload(int damage, int chance, float damageMultiplier)
        {
            this.Damage = damage;
            this.Chance = chance;
            this.Multiplier = damageMultiplier;
        }
    }
}