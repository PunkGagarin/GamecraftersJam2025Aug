using Jam.Scripts.Gameplay.Artifacts.Data;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealIncreaseFactory :
        BaseArtifactFactory<ZenjectArtifactHealIncreaseFactory, ArtifactHealIncreaseSystem>
    {

        public override ArtifactType Type => ArtifactType.HealIncrease;
    }
}