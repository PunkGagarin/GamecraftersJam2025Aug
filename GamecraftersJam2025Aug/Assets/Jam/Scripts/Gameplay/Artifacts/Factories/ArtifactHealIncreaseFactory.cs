using Jam.Scripts.Gameplay.Artifacts.Data;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealIncreaseFactory :
        BaseArtifactFactory<ZenjectArtifactHealOnCritFactory, ArtifactHealOnCritSystem>
    {

        public override ArtifactType Type => ArtifactType.HealIncrease;
    }
}