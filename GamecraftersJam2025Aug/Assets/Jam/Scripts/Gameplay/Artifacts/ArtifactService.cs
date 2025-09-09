using System;
using Jam.Scripts.Gameplay.Artifacts.Data;
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

        public void AddArtifact(ArtifactType type)
        {
            Debug.Log("Adding artifact " + type);
            if (_model.HasArtifact(type))
            {
                Debug.LogError("Trying to add already existing artifact");
                return;
            }

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