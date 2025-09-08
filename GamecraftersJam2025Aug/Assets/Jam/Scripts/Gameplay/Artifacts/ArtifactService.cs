using System;
using Jam.Scripts.Gameplay.Artifacts.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactService : IInitializable
    {
        [Inject] private ArtifactBus _bus;
        [Inject] private ArtifactModelFactory _factory;

        PlayerArtifactsModel _model;

        public void Initialize()
        {
            _model = new();
        }

        public void AddArtifact(ArtifactType type)
        {
            ArtifactModel model = _factory.Create(type);
            _model.AddArtifact(model);
            _bus.AddArtifactInvoke(new ArtifactDto(type, model.Sprite, model.Description));
        }

        public void RemoveArtifact(ArtifactType type)
        {
            _model.RemoveArtifact(type);
            _bus.RemoveArtifactInvoke(type);
        }

    }
}