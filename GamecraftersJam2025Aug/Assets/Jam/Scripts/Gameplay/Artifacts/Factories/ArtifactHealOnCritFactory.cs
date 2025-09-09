using Jam.Scripts.Gameplay.Artifacts.Data;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealOnCritFactory :
        BaseArtifactFactory<ZenjectArtifactHealOnCritFactory, ArtifactHealOnCritSystem>
    {

        public override ArtifactType Type => ArtifactType.HealOnCritical;
    }
}