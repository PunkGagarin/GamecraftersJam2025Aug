using System.Collections.Generic;
using System.Linq;
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

        public  void Check()
        {
            var firstGradeBalls = AllPlayerBalls.Where(b => b.Grade == 1).ToList();
            foreach (var ball in firstGradeBalls)
            {
                if (!AllPlayerBalls.Exists(b => b.BallType == ball.BallType && b.Grade == ball.Grade + 1))
                    Debug.LogError($"Cant find upgrade for ball {ball.BallType}");
            }
        }
    }

}