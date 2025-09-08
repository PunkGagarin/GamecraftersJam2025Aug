using System;
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

        private void AddRandomArtifact(ArtifactSo so)
        {
            _service.AddArtifact(so.Type);
        }
    }
}