using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactDamageAfterQueueShuffleIncreaseSo", fileName = "DamageAfterQueueShuffleIncreaseSo", order = 0)]
    public class ArtifactDamageAfterQueueShuffleIncreaseSo : ArtifactSo
    {
        [field: SerializeField]
        public int DamageIncrease { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.DamageAfterQueueShuffle;
        }
    }
}