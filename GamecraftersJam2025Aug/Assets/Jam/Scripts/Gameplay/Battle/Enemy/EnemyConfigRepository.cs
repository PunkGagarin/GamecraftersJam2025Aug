using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    // [CreateAssetMenu(menuName = "Gameplay/Enemies/Repository", fileName = "EnemyConfigRepository", order = 0)]
    public class EnemyConfigRepository : ScriptableObject
    {
        [field: SerializeField]
        public List<EnemySo> Enemies { get; private set; }

        public EnemySo GetRandomEnemy()
        {
            return Enemies[Random.Range(0, Enemies.Count)];
        }

    }
}