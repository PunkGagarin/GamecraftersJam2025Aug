using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;

namespace Jam.Prefabs.Gameplay.Gold
{
    // [CreateAssetMenu(menuName = "Gameplay/GoldConfig", fileName = "GoldConfig", order = 0)]
    public class GoldConfig : ScriptableObject
    {
        [field: SerializeField]
        public int FirstGradeBallPrice { get; private set; }

        [field: SerializeField]
        public int SecondGradeBallPrice { get; private set; }

        [field: SerializeField]
        public int UpgradeBallPrice { get; private set; }

        [field: SerializeField]
        public int HealPrice { get; private set; }

        [field: SerializeField]
        public int ArtifactPrice { get; private set; }

        [field: Header(" рандомный ")]
        [field: SerializeField]
        public int Gap { get; private set; } = 10;

        [field: SerializeField]
        public List<CustomKeyValue<EnemyTier, int>> GoldPerEnemy { get; private set; }

        public int GetPriceByTypeAndGrade(int dtoGrade)
        {
            return dtoGrade == 1 ? FirstGradeBallPrice : SecondGradeBallPrice;
        }
    }
}