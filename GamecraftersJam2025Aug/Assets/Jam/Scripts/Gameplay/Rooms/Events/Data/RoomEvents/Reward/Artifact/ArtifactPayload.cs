using Jam.Scripts.Gameplay.Artifacts;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class ArtifactPayload : IRewardPayload
    {
        public ArtifactPayload(ArtifactType artifactType) => ArtifactType = artifactType;
        public ArtifactType ArtifactType {get; private set;}
    }
}