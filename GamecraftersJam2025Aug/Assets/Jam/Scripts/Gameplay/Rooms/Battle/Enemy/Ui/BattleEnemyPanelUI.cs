using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class BattleEnemyPanelUI : MonoBehaviour
    {
        
        [field: SerializeField]
        public List<Transform> EnemyPoints { get; private set; }
        
        [field: SerializeField]
        public List<EnemyView> EnemyViews { get; private set; }
        
    }
}