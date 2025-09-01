using System;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameButtonUi : MonoBehaviour
    {
        [field: SerializeField]
        public Button ChooseOneButton { get; private set; }

        [field: SerializeField]
        public Button ChooseTwoButton { get; private set; }

        [field: SerializeField]
        public Button ChooseThreeButton { get; private set; }
        
        public event Action<int> OnBallChosen;

        private void Awake()
        {
            ChooseOneButton.onClick.AddListener(() => OnBallChosen?.Invoke(1));
            ChooseTwoButton.onClick.AddListener(() => OnBallChosen?.Invoke(2));
            ChooseThreeButton.onClick.AddListener(() => OnBallChosen?.Invoke(3));
        }

        public void TurnOnButtonInteraction()
        {
            ChooseOneButton.gameObject.SetActive(true);
            ChooseTwoButton.gameObject.SetActive(true);
            ChooseThreeButton.gameObject.SetActive(true);
        }

        public void TurnOffButtonInteraction()
        {
            ChooseOneButton.gameObject.SetActive(false);
            ChooseTwoButton.gameObject.SetActive(false);
            ChooseThreeButton.gameObject.SetActive(false);
        }
    }
}