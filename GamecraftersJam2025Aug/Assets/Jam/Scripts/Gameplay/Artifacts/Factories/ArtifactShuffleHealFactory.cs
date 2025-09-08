using Jam.Scripts.Gameplay.Artifacts.Data;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactShuffleHealFactory :
        BaseArtifactFactory<ZenjectArtifactShuffleHealFactory, ArtifactShuffleReloadHealSystem>
    {
        public override ArtifactType Type => ArtifactType.HealOnShuffle;
    }
}