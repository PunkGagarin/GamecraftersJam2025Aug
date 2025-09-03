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
            _rewardUi.ToMapButton.onClick.AddListener(OpenMap);
            _loseUi.GameOverButton.onClick.AddListener(FinishGame);
        }

        public void Dispose()
        {
            _bus.OnWin -= ShowWinScreen;
            _bus.OnLose += ShowLoseScreen;
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
            //todo: maji openMap?
            Debug.LogError("Open map IMPLEMENT ME?");
        }
    }

}