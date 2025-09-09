using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactMaxHpEndBattleIncreaseSo", fileName = "MaxHpEndBattleIncreaseSo", order = 0)]
    public class ArtifactMaxHpEndBattleIncreaseSo : ArtifactSo
    {

        [field: SerializeField]
        public int HpIncrease { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.MaxHpEndBattleIncrease;
        }
    }
}