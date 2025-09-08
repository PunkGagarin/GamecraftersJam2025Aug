using Jam.Scripts.Gameplay.Artifacts.Data;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealFromDamageFactory :
        BaseArtifactFactory<ZenjectArtifactHealFromDamageFactory, ArtifactHealFromDamageSystem>
    {

        public override ArtifactType Type => ArtifactType.HealFromDamage;
    }
}