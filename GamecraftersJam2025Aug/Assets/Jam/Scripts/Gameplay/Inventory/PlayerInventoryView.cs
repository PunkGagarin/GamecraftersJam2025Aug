using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class PlayerInventoryView : MonoBehaviour
    {
        [field: SerializeField]
        public Button OpenButton { get; private set; }
        
        [field: SerializeField]
        public Button CloseButton { get; private set; }
        
        [field: SerializeField]
        public Transform  BallsContainer { get; private set; }
        
        
    }
}