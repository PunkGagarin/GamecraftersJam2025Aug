using Jam.Scripts.Gameplay.Artifacts.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public interface IArtifactFactory
    {
        void Create(ArtifactSo artifactSo);
        ArtifactType Type { get; }
    }
}