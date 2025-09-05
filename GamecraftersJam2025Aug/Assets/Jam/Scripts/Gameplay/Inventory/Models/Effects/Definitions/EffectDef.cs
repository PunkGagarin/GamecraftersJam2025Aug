using System;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models.Definitions
{
    [Serializable]
    public abstract class EffectDef
    {

        [field: SerializeField]
        public virtual TargetType Targeting { get; set; } = TargetType.First;
        public abstract EffectInstance ToInstance();

    }

}