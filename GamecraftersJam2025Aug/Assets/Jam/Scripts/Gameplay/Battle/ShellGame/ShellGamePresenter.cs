using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGamePresenter : IInitializable, IDisposable
    {
        [Inject] private readonly ShellGameButtonUi _buttonUi;
        [Inject] private readonly ShellGameView _view;
        [Inject] private readonly ShellGameEventBus _bus;
        [Inject] private readonly ShellGameConfig _shellGameConfig;

        [Inject] private readonly BattleEventBus _battleBus;
        [Inject] private readonly BattleSystem _battleSystem;

        private int _currentTryCount = 0;
        private int _thisTurnTryCount = 2;

        public void Initialize()
        {
            _bus.OnInit += InitShellGame;
            _bus.OnRoundBallsChoosen += InitRoundBalls;
            _battleBus.OnBattleStateChanged += OnShellGameStarted;
            _buttonUi.OnBallChosen += OnPlayerBallCountChoose;
            _view.OnCupClicked += OnCupClicked;
        }

        public void Dispose()
        {
            _bus.OnInit -= InitShellGame;
            _bus.OnRoundBallsChoosen += InitRoundBalls;
            _battleBus.OnBattleStateChanged -= OnShellGameStarted;
            _buttonUi.OnBallChosen -= OnPlayerBallCountChoose;
            _view.OnCupClicked -= OnCupClicked;
        }

        private void InitRoundBalls(List<BallDto> balls)
        {
            _view.InitRoundBalls(balls);
        }

        private void OnPlayerBallCountChoose(int ballCount)
        {
            _thisTurnTryCount = ballCount;
            Shuffle(ballCount);
        }

        private void Shuffle(int ballsCount)
        {
            var config = _shellGameConfig.GetInfoFor(ballsCount);
            _view.PrepareForShuffle(ballsCount, config);

            _battleSystem.PrepareBallsForNextShuffle(ballsCount);

            _currentTryCount = 0;
            _buttonUi.TurnOffButtonInteraction();
            _view.Shuffle();
        }

        private void InitShellGame()
        {
            _view.Init(_shellGameConfig);
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

                if (cupView.BallView == null || cupView.BallView.UnitType == BallUnitType.None)
                {
                    FinishGame();
                    return;
                }
                else if (cupView.BallView.UnitType == BallUnitType.Enemy)
                    BoostEnemy(cupView);
                else
                    _battleSystem.ChooseBall(cupView.BallView.BallId);

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
            _view.Unsubscribe();
        }
    }
}