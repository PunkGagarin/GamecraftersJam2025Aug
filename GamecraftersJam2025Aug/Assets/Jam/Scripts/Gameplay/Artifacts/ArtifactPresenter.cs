using System;
using Jam.Scripts.Gameplay.Artifacts.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactPresenter : IInitializable, IDisposable
    {
        [Inject] private ArtifactTestView _view;
        [Inject] private ArtifactService _service;

        public void Initialize()
        {
            _view.OnArtifactAdded += AddRandomArtifact;
        }

        public void Dispose()
        {
            _view.OnArtifactAdded -= AddRandomArtifact;
        }

        private void AddRandomArtifact(ArtifactType type)
        {
            _service.AddArtifact(type);
        }
    }
}