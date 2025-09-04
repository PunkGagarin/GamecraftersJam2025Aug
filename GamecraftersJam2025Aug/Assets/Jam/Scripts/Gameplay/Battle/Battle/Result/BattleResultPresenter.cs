using System;
using Jam.Scripts.MapFeature.Map.Domain;
using Jam.Scripts.SceneManagement;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleResultPresenter : IInitializable, IDisposable
    {
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private MapEventBus _mapEventBus;
        [Inject] private BattleRewardUi _rewardUi;
        [Inject] private BattleLoseUi _loseUi;
        [Inject] private SceneChanger _sceneChanger;

        public void Initialize()
        {
            _battleEventBus.OnWin += ShowWinScreen;
            _battleEventBus.OnLose += ShowLoseScreen;
            _rewardUi.ToMapButton.onClick.AddListener(OpenMap);
            _loseUi.GameOverButton.onClick.AddListener(FinishGame);
        }

        public void Dispose()
        {
            _battleEventBus.OnWin -= ShowWinScreen;
            _battleEventBus.OnLose += ShowLoseScreen;
            _rewardUi.ToMapButton.onClick.RemoveListener(OpenMap);
            _loseUi.GameOverButton.onClick.RemoveListener(FinishGame);
        }

        private void FinishGame()
        {
            _sceneChanger.StartMenu();
        }

        private void ShowWinScreen()
        {
            //todo: results
            _rewardUi.Show();
        }

        private void ShowLoseScreen()
        {
            _loseUi.Show();
        }

        private void OpenMap()
        {
            _rewardUi.Hide();
            _mapEventBus.OnRoomCompleted.Invoke();
        }
    }

}