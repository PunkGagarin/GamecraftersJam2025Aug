using System;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class FinishGameUI : ContentUi, IInitializable, IDisposable
    {

        [Inject] private BattleEventBus _bus;
        [Inject] private SceneChanger _sceneChanger;

        [field: SerializeField]
        public Button FinishButton { get; private set; }

        public void Initialize()
        {
            _bus.OnAllGameFinished += StartFinalGameDialogue;
            FinishButton.onClick.AddListener(RestartGame);
        }

        public void Dispose()
        {
            _bus.OnAllGameFinished -= StartFinalGameDialogue;
            FinishButton.onClick.RemoveListener(RestartGame);
        }

        private void RestartGame()
        {
            _sceneChanger.StartGameplay();
        }

        private void StartFinalGameDialogue()
        {
            Show();
        }
    }
}