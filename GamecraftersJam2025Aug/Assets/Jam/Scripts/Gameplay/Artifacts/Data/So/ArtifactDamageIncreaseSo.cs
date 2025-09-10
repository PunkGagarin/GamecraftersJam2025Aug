using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactDamageIncrease", fileName = "DamageIncreaseSo", order = 0)]
    public class ArtifactDamageIncreaseSo : ArtifactSo
    {

        [field: SerializeField]
        public int DamageIncrease { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.DamageIncrease;
        }
    }
}