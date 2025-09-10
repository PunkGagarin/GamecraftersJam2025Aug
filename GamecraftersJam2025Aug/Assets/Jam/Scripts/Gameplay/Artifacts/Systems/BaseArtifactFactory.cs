using System;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class BaseArtifactFactory<AS> : PlaceholderFactory<ArtifactSo, AS>, IDisposable
        where AS : IArtifactSystem
    {
        private AS _system;

        public override AS Create(ArtifactSo artifactSo)
        {
            _system = base.Create(artifactSo);
            _system.Initialize();
            return _system;
        }

        public void Dispose()
        {
            _system?.Dispose();
        }
    }
}