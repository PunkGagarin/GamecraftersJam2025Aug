using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class BattleLoseUi : ContentUi
    {
        
        [field: SerializeField]
        public Button GameOverButton { get; private set; }

    }
}