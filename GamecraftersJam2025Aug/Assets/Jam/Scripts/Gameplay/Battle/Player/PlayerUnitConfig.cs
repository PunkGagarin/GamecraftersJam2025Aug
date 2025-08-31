using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    // [CreateAssetMenu(menuName = "Gameplay/Player/UnitConfig", fileName = "PlayerUnitConfig", order = 0)]
    public class PlayerUnitConfig : ScriptableObject
    {

        [field: SerializeField]
        public int StartHealth { get; private set; } = 100;
        
        
    }
}