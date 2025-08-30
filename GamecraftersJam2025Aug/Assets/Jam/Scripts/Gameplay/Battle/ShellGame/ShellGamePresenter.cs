using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGamePresenter : IInitializable, IDisposable
    {
        [Inject] private readonly ShellGameButtonUi _buttonUi;
        [Inject] private readonly ShellGameManagerView _view;
        [Inject] private readonly ShellGameEventBus _bus;
        [Inject] private readonly ShellGameConfig _shellGameConfig;

        [Inject] private readonly BattleEventBus _battleBus;
        [Inject] private readonly BattleSystem _battleSystem;

        private int _currentTryCount = 0;
        private int _thisTurnTryCount = 2;
        private int _ballsCount = 2;

        public void Initialize()
        {
            _bus.OnInit += InitShellGame;
            _battleBus.OnBattleStateChanged += OnShellGameStarted;
            _buttonUi.ChooseAttackButton.onClick.AddListener(Shuffle);
            _view.OnCupClicked += OnCupClicked;

            _thisTurnTryCount = _shellGameConfig.TryCount;
        }

        public void Dispose()
        {
            _bus.OnInit -= InitShellGame;
            _battleBus.OnBattleStateChanged -= OnShellGameStarted;
            _buttonUi.ChooseAttackButton.onClick.RemoveListener(Shuffle);
            _view.OnCupClicked -= OnCupClicked;
        }

        private void Shuffle()
        {
            _currentTryCount = 0;
            _buttonUi.TurnOffButtonInteraction();
            _view.Shuffle();
        }

        private void InitShellGame()
        {
            List<int> ballIds = GetNextRoundBallIds();
            _view.Init(_shellGameConfig);
        }

        private List<int> GetNextRoundBallIds()
        {
            var ballsCount = _ballsCount;
            //go somewhere and get Ids
            var ballIdsList = new List<int>();
            
            for (int i = 1; i <= ballsCount; i++)
                ballIdsList.Add(i);

            //send request into BattleQueueService
            return ballIdsList;
        }

        private void OnPlayerAttackChosen()
        {
            _buttonUi.TurnOffButtonInteraction();
            _battleSystem.ChooseBall(1);
        }

        private void OnShellGameStarted(BattleState state)
        {
            if (state != BattleState.ShellGame)
                return;

            _buttonUi.TurnOnButtonInteraction();
        }


        private void OnCupClicked(CupView cupView)
        {
            if (_currentTryCount < _thisTurnTryCount)
            {
                _currentTryCount++;
                cupView.ShowBall();

                if (cupView.BallView == null || cupView.BallView.Type == MyBallType.None)
                {
                    FinishGame();
                    return;
                }
                else if (cupView.BallView.Type == MyBallType.Enemy)
                    BoostEnemy(cupView);
                else
                    _battleSystem.ChooseBall(1);

                if (_currentTryCount >= _thisTurnTryCount)
                    FinishGame();
            }
            else
                Debug.LogError("а схренали мы кликаем за пределами каунта?");
        }

        private void BoostEnemy(CupView cupView)
        {
            Debug.Log($" вытащили шарик врага, бустим врага (пока нет)");
        }

        private async Task FinishGame()
        {
            _buttonUi.TurnOffButtonInteraction();
            _view.ShowBallsForAllCups();
            _battleSystem.StartPlayerTurn();
        }
    }
}