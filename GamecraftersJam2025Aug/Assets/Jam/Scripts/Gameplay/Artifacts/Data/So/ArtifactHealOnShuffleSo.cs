using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/HealOnShuffle", fileName = "HealOnShuffle", order = 0)]
    public class ArtifactHealOnShuffleSo : ArtifactSo
    {
        [field:SerializeField] 
        public int HealAmount { get; private set; }
        
        private void Awake()
        {
            Type = ArtifactType.HealOnShuffle;
        }
    }
}