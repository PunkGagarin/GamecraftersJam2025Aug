using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models.Definitions
{
    [Serializable]
    public sealed class HealEffectDef : EffectDef
    {
        [field: Header("Heal")]
        [field: SerializeField]
        public int Amount { get; set; }

        public override EffectInstance ToInstance()
        {
            return new(Targeting, new HealPayload(Amount));
        }
    }
}