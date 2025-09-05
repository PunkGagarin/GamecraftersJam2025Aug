using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models.Definitions
{
    [Serializable]
    public sealed class DamageEffectDef : EffectDef
    {

        [field: Header("Damage")]
        [field: SerializeField]
        public int Amount { get; set; }

        public override EffectInstance ToInstance() =>
            new(Targeting, new DamagePayload(Amount));
    }
}