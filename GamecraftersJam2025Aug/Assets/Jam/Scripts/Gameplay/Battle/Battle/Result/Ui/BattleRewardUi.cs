using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleRewardUi : ContentUi
    {
                
        [field: SerializeField]
        public Button ToMapButton { get; private set; }
    }
}