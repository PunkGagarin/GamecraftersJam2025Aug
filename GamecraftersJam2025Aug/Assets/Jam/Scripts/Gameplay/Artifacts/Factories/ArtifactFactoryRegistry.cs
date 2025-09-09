using System.Collections.Generic;
using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactFactoryRegistry : IInitializable
    {
        [Inject] private readonly ArtifactSoRepository _repository;
        [Inject] private readonly DiContainer _diContainer;

        public Dictionary<ArtifactType, IFactory<ArtifactSo, IArtifactSystem>> _factories;

        [Inject]
        public List<IFactory<ArtifactSo, IArtifactSystem>> _factories2;

        public void Initialize()
        {
            _factories = new()
            {
                { ArtifactType.HealOnShuffle, _diContainer.Resolve<ArtifactHealOnShuffleSystem.ArtifactFactory>() },
                { ArtifactType.HealOnCritical, _diContainer.Resolve<ArtifactHealOnCritSystem.ArtifactFactory>() },
                { ArtifactType.HealIncrease, _diContainer.Resolve<ArtifactHealIncreaseSystem.ArtifactFactory>() },
                {
                    ArtifactType.HealFromDamage,
                    _diContainer.Resolve<ArtifactHealFromDamageSystem.ArtifactFactory>()
                },
            };
        }

        public void CreateArtifactSystem(ArtifactSo so)
        {
            Debug.LogError($" _factories2 count: {_factories2.Count}");
            var factory = _factories[so.Type];
            factory.Create(so);
        }
    }
}