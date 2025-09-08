using System.Linq;
using Jam.Scripts.Gameplay.Artifacts.Data;
using Jam.Scripts.GameplayData.Repositories;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Artifacts
{
    // [CreateAssetMenu(menuName = "Gameplay/Artifact/ArtifactSoRepository", fileName = "ArtifactSoRepository", order = 0)]
    public class ArtifactSoRepository : Repository<ArtifactSo>
    {
        public ArtifactSo GetArtifactSo(ArtifactType type)
        {
            var art = Definitions.FirstOrDefault(a => a.Type == type);

            if (art == null)
            {
                Debug.LogError($" There is no artifact with type {type} in repository. ");
                return null;
            }

            return art;
        }
    }
}