using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactDto
    {
        public ArtifactType Type { get; set; }
        public Sprite Sprite { get; set; }
        public string Description { get; set; }

        public ArtifactDto(ArtifactType type, Sprite sprite, string description)
        {
            Type = type;
            Sprite = sprite;
            Description = description;
        }
    }
}