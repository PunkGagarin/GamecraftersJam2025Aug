using System;
using Jam.Scripts.Gameplay.Artifacts.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public interface IArtifactSystem : IInitializable, IDisposable
    {
        public void Execute();
        public void Init(ArtifactSo data);
    }
}