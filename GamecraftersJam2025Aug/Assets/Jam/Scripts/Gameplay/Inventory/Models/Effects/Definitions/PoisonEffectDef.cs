using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models.Definitions
{
    [Serializable]
    public sealed class PoisonEffectDef : EffectDef
    {
        [field: Header("Poison")]
        [field: SerializeField]
        public int Damage { get; set; }

        public override EffectInstance ToInstance() =>
            new(Targeting, new PoisonPayload(Damage));
    }
}