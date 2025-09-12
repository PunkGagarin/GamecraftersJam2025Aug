using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class HealWinButton : MonoBehaviour
    {
        [field: SerializeField]
        public Button HealButton { get; private set; }  
        
        [field: SerializeField]
        public TextMeshProUGUI PriceText { get; private set; }
    }
}