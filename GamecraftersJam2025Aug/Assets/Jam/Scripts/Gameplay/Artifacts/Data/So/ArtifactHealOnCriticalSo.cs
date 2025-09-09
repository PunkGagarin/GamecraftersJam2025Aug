using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/HealOnCritical", fileName = "HealOnCritical", order = 0)]
    public class ArtifactHealOnCriticalSo : ArtifactSo
    {

        [field: SerializeField]
        public int HealAmount { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.HealOnCritical;
        }
    }
}