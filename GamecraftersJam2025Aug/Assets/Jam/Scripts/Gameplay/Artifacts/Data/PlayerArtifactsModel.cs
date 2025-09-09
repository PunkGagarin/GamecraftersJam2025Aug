using System.Collections.Generic;
using Jam.Scripts.Gameplay.Artifacts.Data;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class PlayerArtifactsModel
    {
        private List<ArtifactModel> Artifacts = new();

        public void AddArtifact(ArtifactModel artifact)
        {
            Artifacts.Add(artifact);
        }

        public void RemoveArtifact(ArtifactType type)
        {
            Artifacts.RemoveAll(a => a.Type == type);
        }

        public bool HasArtifact(ArtifactType type)
        {
            return Artifacts.Exists(a => a.Type == type);
        }
    }
}