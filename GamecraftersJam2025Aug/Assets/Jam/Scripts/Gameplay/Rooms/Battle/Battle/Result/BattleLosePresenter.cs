using System;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.MapFeature.Map.Domain;
using Jam.Scripts.SceneManagement;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class BattleLosePresenter : IInitializable, IDisposable
    {
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private PlayerEventBus _playerBus;
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private BattleLoseUi _loseUi;
        [Inject] private SceneChanger _sceneChanger;

        public void Initialize()
        {
            _battleEventBus.OnLose += ShowLoseScreen;
            _loseUi.GameOverButton.onClick.AddListener(FinishGame);
            _playerBus.OnDeath += ShowLoseScreen;
        }

        public void Dispose()
        {
            _battleEventBus.OnLose += ShowLoseScreen;
            _loseUi.GameOverButton.onClick.RemoveListener(FinishGame);
            _playerBus.OnDeath -= ShowLoseScreen;
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