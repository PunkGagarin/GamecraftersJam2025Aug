using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactFactoryRegistry
    {
        [Inject] private readonly ArtifactSoRepository _repository;

        [Inject]
        public readonly List<IArtifactFactory> _factories = new();

        public void CreateArtifactSystem(ArtifactSo so)
        {
            var factory = _factories.FirstOrDefault(el => el.Type == so.Type);
            factory.Create(so);
        }
    }
}