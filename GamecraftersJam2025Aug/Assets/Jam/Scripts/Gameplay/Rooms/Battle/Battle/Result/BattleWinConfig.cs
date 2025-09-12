using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    // [CreateAssetMenu(menuName = "Gameplay/BattleWinConfig", fileName = "BattleWinConfig", order = 0)]
    public class BattleWinConfig : ScriptableObject
    {
        [field: SerializeField]
        public int HealAmountPercent { get; private set; } = 50;

        [field: SerializeField]
        public int SecondGradeWeight { get; private set; } = 10;
    }
}