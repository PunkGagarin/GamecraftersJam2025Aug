using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactHealIncreaseSo", fileName = "ArtifactHealIncreaseSo", order = 0)]
    public class ArtifactHealIncreaseSo : ArtifactSo
    {

        [field: SerializeField]
        public int IncreaseAmount { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.HealIncrease;
        }
    }
}