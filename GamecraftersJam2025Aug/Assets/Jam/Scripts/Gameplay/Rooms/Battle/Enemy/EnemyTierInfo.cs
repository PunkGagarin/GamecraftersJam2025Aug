using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    [Serializable]
    public class EnemyTierInfo
    {
        [field: SerializeField]
        public CustomKeyValue<int, List<CustomKeyValue<EnemyTier, int>>> TierInfo { get; set; }
    }
}