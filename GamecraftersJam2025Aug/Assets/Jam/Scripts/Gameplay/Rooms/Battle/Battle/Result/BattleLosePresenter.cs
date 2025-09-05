using System;
using Jam.Scripts.MapFeature.Map.Domain;
using Jam.Scripts.SceneManagement;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleLosePresenter : IInitializable, IDisposable
    {
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private BattleLoseUi _loseUi;
        [Inject] private SceneChanger _sceneChanger;

        public void Initialize()
        {
            _battleEventBus.OnLose += ShowLoseScreen;
            _loseUi.GameOverButton.onClick.AddListener(FinishGame);
        }

        public void Dispose()
        {
            _battleEventBus.OnLose += ShowLoseScreen;
            _loseUi.GameOverButton.onClick.RemoveListener(FinishGame);
        }

        private void FinishGame()
        {
            _sceneChanger.StartMenu();
        }

        private void ShowLoseScreen()
        {
            _loseUi.Show();
        }
    }
}