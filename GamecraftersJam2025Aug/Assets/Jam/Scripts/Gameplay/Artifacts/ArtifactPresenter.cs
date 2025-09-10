using System;
using Jam.Scripts.Gameplay.Artifacts.Views;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactPresenter : IInitializable, IDisposable
    {
        [Inject] private ArtifactsUi _view;
        [Inject] private ArtifactService _service;
        [Inject] private ArtifactBus _bus;

        public void Initialize()
        {
            _view.OnArtifactAdded += AddRandomArtifact;
            _bus.OnArtifactAdded += AddArtifactView;
        }

        public void Dispose()
        {
            _view.OnArtifactAdded -= AddRandomArtifact;
            _bus.OnArtifactAdded -= AddArtifactView;
        }

        private void AddArtifactView(ArtifactDto dto)
        {
            _view.AddArtifact(dto);
        }

        private void AddRandomArtifact(ArtifactType type)
        {
            _service.AddArtifact(type);
        }
    }
}