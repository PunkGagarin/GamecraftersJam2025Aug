using Jam.Scripts.GameplayData.Definitions;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{

    public class ArtifactSo : Definition
    {
        [field: SerializeField]
        public virtual ArtifactType Type { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeField]
        public string Description { get; set; }
    }
}