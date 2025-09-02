using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameButtonUi : MonoBehaviour
    {
        //добавить хинт для эдитора
        [field: Tooltip("ПОРЯДОК ВАЖЕН!!")]
        [field: SerializeField]
        private List<Button> ChooseButtons { get; set; }

        [field: SerializeField]
        public Button StartShuffleButton { get; private set; }

        public event Action<int> OnBallChosen;

        private void Awake()
        {
            for (int i = 0; i < ChooseButtons.Count; i++)
            {
                //интересный кейс с замыканием
                int i1 = i;
                ChooseButtons[i].onClick.AddListener(() => OnBallChosen?.Invoke(i1 + 1));
            }
        }


        public void ShowChooseButtonInteraction(int ballsInQueue)
        {
            HideShuffleButton();
            int buttonsToTurnOn = 3;
            if (ballsInQueue < buttonsToTurnOn)
                buttonsToTurnOn = ballsInQueue;

            for (int i = 0; i < buttonsToTurnOn; i++)
                ChooseButtons[i].gameObject.SetActive(true);
        }

        public void HideChooseButtonInteraction()
        {
            foreach (var button in ChooseButtons)
                button.gameObject.SetActive(false);
        }

        public void HideShuffleButton()
        {
            StartShuffleButton.gameObject.SetActive(false);
        }

        public void ShowShuffleButton()
        {
            StartShuffleButton.gameObject.SetActive(true);
        }
    }
}