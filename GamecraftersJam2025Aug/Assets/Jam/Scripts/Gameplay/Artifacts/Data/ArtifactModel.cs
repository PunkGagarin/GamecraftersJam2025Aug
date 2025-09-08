using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactModel
    {

        public ArtifactType Type { get; set; }
        public Sprite Sprite { get; set; }
        public string Description { get; set; }

        public ArtifactModel(ArtifactType type, Sprite sprite, string description)
        {
            Type = type;
            Sprite = sprite;
            Description = description;
        }
    }
}