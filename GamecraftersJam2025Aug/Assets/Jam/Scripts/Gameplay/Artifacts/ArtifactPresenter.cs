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
        [Inject] private BallDescriptionUi _descUi;

        public void Initialize()
        {
            _view.OnArtifactAdded += AddRandomArtifact;
            _view.OnMouseEnter += ShowDesc;
            _view.OnMouseExit += _descUi.Hide;
            _bus.OnArtifactAdded += AddArtifactView;
        }

        public void Dispose()
        {
            _view.OnArtifactAdded -= AddRandomArtifact;
            _bus.OnArtifactAdded -= AddArtifactView;
            _view.OnMouseEnter -= ShowDesc;
            _view.OnMouseExit -= _descUi.Hide;
        }

        
        private void ShowDesc(string obj)
        {
            _descUi.SetDesc(obj);
            _descUi.Show();
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