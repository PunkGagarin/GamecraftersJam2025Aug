using System;
using Jam.Scripts.Gameplay.Artifacts.Data;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactBus
    {
        public event Action<ArtifactType> OnArtifactRemoved = delegate { };
        public event Action<ArtifactDto> OnArtifactAdded = delegate { };
        
        public void RemoveArtifactInvoke(ArtifactType type) => OnArtifactRemoved.Invoke(type);
        public void AddArtifactInvoke(ArtifactDto dto) => OnArtifactAdded.Invoke(dto);

    }

}