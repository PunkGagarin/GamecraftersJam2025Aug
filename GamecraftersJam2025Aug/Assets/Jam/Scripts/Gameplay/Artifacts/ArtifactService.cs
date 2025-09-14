using System;
using System.Linq;
using UnityEngine;
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
        
        public ArtifactType GetRandomArtifactType()
        {
            var existedArtifacts = _model.Artifacts.Select(a => a.Type).ToList();
            return _factory.GetNonExistedArtifactType(existedArtifacts);
        }

        public void AddArtifact(ArtifactType type)
        {
            Debug.Log("Adding artifact " + type);
            if (_model.HasArtifact(type))
            {
                Debug.LogError("Trying to add already existing artifact");
                return;
            }

            var model = GetArtifactModelByType(type);
            _model.AddArtifact(model);
            _bus.AddArtifactInvoke(GetArtifactDto(type, model));
        }

        private ArtifactModel GetArtifactModelByType(ArtifactType type) => 
            _factory.Create(type);

        private static ArtifactDto GetArtifactDto(ArtifactType type, ArtifactModel model) => 
            new(type, model.Sprite, model.Description);

        public ArtifactDto GetArtifactDtoByType(ArtifactType type)
        {
            var model = GetArtifactModelByType(type);
            var dto = GetArtifactDto(type, model);
            return dto;
        }
        
        public void RemoveArtifact(ArtifactType type)
        {
            _model.RemoveArtifact(type);
            _bus.RemoveArtifactInvoke(type);
        }

    }
}