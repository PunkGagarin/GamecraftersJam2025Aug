using System;
using Jam.Scripts.SceneManagement;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleResultPresenter : IInitializable, IDisposable
    {
        [Inject] private BattleEventBus _bus;
        [Inject] private BattleRewardUi _rewardUi;
        [Inject] private BattleLoseUi _loseUi;
        [Inject] private SceneChanger _sceneChanger;

        public void Initialize()
        {
            _bus.OnWin += ShowWinScreen;
            _bus.OnLose += ShowLoseScreen;
            _loseUi.GameOverButton.onClick.AddListener(FinishGame);
            _rewardUi.ToMapButton.onClick.AddListener(OpenMap);
        }

        public void Dispose()
        {
            _bus.OnWin -= ShowWinScreen;
            _bus.OnLose += ShowLoseScreen;
            _loseUi.GameOverButton.onClick.RemoveListener(FinishGame);
            _rewardUi.ToMapButton.onClick.RemoveListener(OpenMap);
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
            //todo: maji openMap?
            Debug.LogError("Open map IMPLEMENT ME?");
        }
    }

}