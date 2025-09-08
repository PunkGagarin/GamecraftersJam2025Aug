using System;
using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public abstract class BaseArtifactFactory<ZFactory, ASystem> : IArtifactFactory, IDisposable
        where ZFactory : IFactory<ASystem> where ASystem : IArtifactSystem
    {
        [Inject] ZFactory _factory;

        private ASystem _system;
        public virtual ArtifactType Type { get; protected set; }

        public void Create(ArtifactSo artifactSo)
        {
            _system = _factory.Create();
            _system.Initialize();
            _system.Init(artifactSo);
        }

        public void Dispose()
        {
            _system?.Dispose();
        }
    }
}

