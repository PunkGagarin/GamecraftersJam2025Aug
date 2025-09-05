using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models.Definitions
{
    [Serializable]
    public class ShieldEffectDef : EffectDef
    {
        [field: Header("Shield")]
        [field: SerializeField]
        public int Amount { get; set; }

        public override EffectInstance ToInstance()
        {
            return new(Targeting, new ShieldPayload(Amount));
        }
    }
}