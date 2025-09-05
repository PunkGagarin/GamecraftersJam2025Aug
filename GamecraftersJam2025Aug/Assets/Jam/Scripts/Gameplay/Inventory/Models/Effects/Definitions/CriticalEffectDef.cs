using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models.Definitions
{
    public class CriticalEffectDef : EffectDef
    {
        [field:Header("Crit")]
        [field:SerializeField]
        public int Damage { get; set; } 
        [field:SerializeField]
        public int Chance { get; set; } 
        
        [field:SerializeField]
        public float CritDamage { get; set; }
        
        public override EffectInstance ToInstance()
        {
            return new(Targeting, new CriticalDamagePayload(Damage, Chance, CritDamage));
        }
    }
}