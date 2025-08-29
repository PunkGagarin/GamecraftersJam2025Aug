using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    /// <summary>
    /// Не уверен стоит ли это в этом классе оставлять или вынести в Player view
    /// </summary>
    public class ShellGameView : MonoBehaviour
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