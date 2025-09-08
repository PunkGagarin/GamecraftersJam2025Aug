using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactHealFromDamageSo", fileName = "HealFromDamageSo", order = 0)]
    public class ArtifactHealFromDamageSo : ArtifactSo
    {

        [field: SerializeField]
        public int HealPercent { get; private set; }

        private void Awake()
        {
            Type = ArtifactType.HealFromDamage;
        }
    }
}