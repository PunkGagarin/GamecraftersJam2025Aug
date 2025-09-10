using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactDamageFromHealSo", fileName = "DamageFromHealSo", order = 0)]
    public class ArtifactDamageFromHealSo: ArtifactSo
    {

        [field: SerializeField]
        public int DamagePercent { get; private set; } = 30;

        private void Awake()
        {
            Type = ArtifactType.DamageFromHeal;
        }
    }
}