using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactDamageAfterKillIncreaseSo", fileName = "DamageAfterKillIncreaseSo", order = 0)]
    public class ArtifactDamageAfterKillIncreaseSo : ArtifactSo
    {

        [field: SerializeField]
        public int DamageIncrease { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.DamageAfterKillIncrease;
        }
    }
}