using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactFactoryRegistry : IInitializable
    {
        [Inject] private readonly ArtifactSoRepository _repository;
        [Inject] private readonly DiContainer _diContainer;

        public Dictionary<ArtifactType, IFactory<ArtifactSo, IArtifactSystem>> _factories;

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
                {
                    ArtifactType.MaxHpEndBattleIncrease,
                    _diContainer.Resolve<ArtifactMaxHpEndBattleIncreaseSystem.ArtifactFactory>()
                },
                {
                    ArtifactType.DamageIncrease,
                    _diContainer.Resolve<ArtifactDamageIncreaseSystem.ArtifactFactory>()
                },
                {
                    ArtifactType.DamageAfterKillIncrease,
                    _diContainer.Resolve<ArtifactDamageAfterKillIncreaseSystem.ArtifactFactory>()
                },
            };
        }

        public void CreateArtifactSystem(ArtifactSo so)
        {
            var factory = _factories[so.Type];
            factory.Create(so);
        }
    }
}