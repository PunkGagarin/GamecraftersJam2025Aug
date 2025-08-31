using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Configs
{
    // [CreateAssetMenu(menuName = "Gameplay/Ball/Repository", fileName = "BallsConfigRepository", order = 0)]
    public class BallsConfigRepository : ScriptableObject
    {

        [field: SerializeField]
        public List<BallSo> DefaultPlayerBalls { get; private set; }
        
        [field: SerializeField]
        public List<BallSo> AllPlayerBalls { get; private set; }
        
        [field: SerializeField]
        public List<BallSo> EnemyBalls { get; private set; }
    }

}