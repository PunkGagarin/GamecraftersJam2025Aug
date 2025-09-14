using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactModelFactory
    {
        [Inject] private ArtifactSoRepository _repository;
        [Inject] private ArtifactFactoryRegistry _factoryRegistry;
        [Inject] private LocalizationTool _locTool;

        public ArtifactModel Create(ArtifactType type)
        {
            ArtifactSo so = _repository.GetArtifactSo(type);
            _factoryRegistry.CreateArtifactSystem(so);
            return new ArtifactModel(type, so.Sprite, _locTool.GetText(so.Description));
        }

        public ArtifactType GetNonExistedArtifactType(List<ArtifactType> existedArtifacts)
        {
            if (existedArtifacts.Count >= _repository.Definitions.Count)
            {
                Debug.LogError(
                    $"All {existedArtifacts.Count} artifacts are existed, there is no non existed artifacts");
                return ArtifactType.None;
            }

            var nonExistedArts = _repository.Definitions.Where(a => !existedArtifacts.Contains(a.Type)).ToList();
            return nonExistedArts[Random.Range(0, nonExistedArts.Count)].Type;
        }
    }
}