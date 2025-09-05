using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using Jam.Scripts.Gameplay.Battle.ShellGame;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.UI.Clown;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.ShellGame
{
    public class ShellGamePresenter : IInitializable, IDisposable
    {
        [Inject] private readonly ShellGameButtonUi _buttonUi;
        [Inject] private readonly ShellGameView _view;
        [Inject] private readonly ShellGameEventBus _bus;
        [Inject] private readonly ShellGameConfig _shellGameConfig;

        [Inject] private readonly BattleEventBus _battleBus;
        [Inject] private readonly RoomEventBus _roomEventBus;
        [Inject] private readonly ClownEventBus _clownEventBus;
        [Inject] private readonly BattleSystem _battleSystem;
        [Inject] private readonly PlayerService _playerService;

        private int _currentTryCount = 0;
        private int _thisTurnTryCount = 2;

        public void Initialize()
        {
            _bus.OnInit += InitShellGame;
            _battleBus.OnShellGameStarted += OnShellGameStarted;
            _bus.OnRoundBallsChoosen += InitRoundBalls;
            _buttonUi.OnBallChosen += OnPlayerBallCountChoose;
            _buttonUi.StartShuffleButton.onClick.AddListener(Shuffle);
            _view.OnCupClicked += OnCupClicked;
            _roomEventBus.OnRoomCompleted += CleanUp;
        }

        public void Dispose()
        {
            _bus.OnInit -= InitShellGame;
            _battleBus.OnShellGameStarted -= OnShellGameStarted;
            _bus.OnRoundBallsChoosen += InitRoundBalls;
            _buttonUi.OnBallChosen -= OnPlayerBallCountChoose;
            _buttonUi.StartShuffleButton.onClick.RemoveListener(Shuffle);
            _view.OnCupClicked -= OnCupClicked;
            _roomEventBus.OnRoomCompleted -= CleanUp;
        }

        private void CleanUp()
        {
            _view.FinishBattleCleanUp();
        }

        private void InitRoundBalls(List<BallDto> balls)
        {
            _view.InitRoundBalls(balls);
        }

        private void OnPlayerBallCountChoose(int ballCount)
        {
            _thisTurnTryCount = ballCount;
            _buttonUi.ShowShuffleButton();
            PrepareForShuffle(ballCount);
        }

        private void Shuffle()
        {
            _currentTryCount = 0;
            _buttonUi.HideShuffleButton();
            _view.Shuffle();
        }

        private void PrepareForShuffle(int ballsCount)
        {
            var config = _shellGameConfig.GetInfoFor(ballsCount);
            var ballDtos = _battleSystem.PrepareBallsForNextShuffle(ballsCount);
            _view.PrepareForShuffle(ballsCount, ballDtos, config);
            _buttonUi.HideChooseButtonInteraction();
        }

        private void InitShellGame()
        {
            _view.Init(_shellGameConfig);
        }

        private void OnShellGameStarted(int ballsInQueueLeft)
        {
            _buttonUi.ShowChooseButtonInteraction(ballsInQueueLeft);
            _view.CleanUp();
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
                {
                    _clownEventBus.UserChoseCupSuccess();                    
                    _battleSystem.ChooseBall(cupView.BallView.BallId);
                }

                if (_currentTryCount >= _thisTurnTryCount)
                    FinishGame();
            }
            else
                Debug.LogError("а схренали мы кликаем за пределами каунта?");
        }

        private void BoostEnemy(CupView cupView)
        {
            _clownEventBus.UserChoseCupFail();
            Debug.Log($" вытащили шарик врага, бустим врага (пока нет)");
        }

        private async void FinishGame()
        {
            _buttonUi.HideChooseButtonInteraction();
            _view.ShowBallsForAllCups();
            _view.Unsubscribe();
            await Task.Delay(500);
            _battleSystem.StartPlayerTurn();
        }
    }
}