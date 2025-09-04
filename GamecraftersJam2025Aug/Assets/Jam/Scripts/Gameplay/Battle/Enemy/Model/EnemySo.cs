using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    [CreateAssetMenu(menuName = "Gameplay/Enemies/EnemySo", fileName = "EnemySo", order = 0)]
    public class EnemySo : ScriptableObject
    {
        [field: SerializeField]
        public EnemyType Type { get; private set; }

        [field: SerializeField]
        public int Health { get; private set; }
        
        [field: SerializeField]
        public int Damage { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }
        
        [field: SerializeField]
        public EnemyTier Tier { get; private set; }
    }

}