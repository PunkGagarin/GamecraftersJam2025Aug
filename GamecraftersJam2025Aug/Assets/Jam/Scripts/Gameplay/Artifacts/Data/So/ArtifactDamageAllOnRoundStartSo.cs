using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactDamageAllOnRoundStartSo", fileName = "DamageAllOnRoundStartSo", order = 0)]
    public class ArtifactDamageAllOnRoundStartSo : ArtifactSo
    {
        [field: SerializeField]
        public int Damage { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.DamageOnRoundStart;
        }
    }
}