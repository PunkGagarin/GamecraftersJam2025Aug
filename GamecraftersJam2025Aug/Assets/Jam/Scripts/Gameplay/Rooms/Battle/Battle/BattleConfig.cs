using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle
{
    // [CreateAssetMenu(menuName = "Gameplay/BattleConfig", fileName = "BattleConfig", order = 0)]
    public class BattleConfig : ScriptableObject
    {
        [field: SerializeField]
        public int MaxEnemiesCountPerWave { get; private set; } = 3;
    }
}