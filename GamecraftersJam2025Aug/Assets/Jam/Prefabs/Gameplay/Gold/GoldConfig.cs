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
        public int UpgradeGradeBallPrice { get; private set; }

        [field: SerializeField]
        public int HealPrice { get; private set; }

        [field: SerializeField]
        public int ArtifactPrice { get; private set; }

        [field: Header(" рандомный ")]
        [field: SerializeField]
        public int PriceGap { get; private set; }
    }
}