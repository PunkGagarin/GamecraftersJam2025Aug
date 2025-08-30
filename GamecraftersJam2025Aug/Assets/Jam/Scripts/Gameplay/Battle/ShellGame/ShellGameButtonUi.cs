using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameButtonUi : MonoBehaviour
    {
        [field: SerializeField]
        public Button ChooseAttackButton { get; private set; }

        public void TurnOnButtonInteraction()
        {
            ChooseAttackButton.interactable = true;
        }

        public void TurnOffButtonInteraction()
        {
            ChooseAttackButton.interactable = false;
        }

    }
}