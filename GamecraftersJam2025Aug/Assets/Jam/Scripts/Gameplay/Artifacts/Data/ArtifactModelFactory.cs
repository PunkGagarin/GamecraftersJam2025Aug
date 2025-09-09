using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactModelFactory
    {
        [Inject] private ArtifactSoRepository _repository;
        [Inject] private ArtifactFactoryRegistry _factoryRegistry;

        public ArtifactModel Create(ArtifactType type)
        {
            ArtifactSo so = _repository.GetArtifactSo(type);
            _factoryRegistry.CreateArtifactSystem(so);
            return new ArtifactModel( type, so.Sprite, so.Description);
        }
    }
}