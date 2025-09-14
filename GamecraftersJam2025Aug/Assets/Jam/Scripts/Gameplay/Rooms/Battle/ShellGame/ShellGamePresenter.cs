using System;
using Cysharp.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.ShellGame;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
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
        [Inject] private readonly RoomRewardBus _roomRewardBus;
        [Inject] private readonly ClownEventBus _clownEventBus;
        [Inject] private readonly BattleSystem _battleSystem;
        [Inject] private readonly PlayerBattleService _playerBattleService;
        [Inject] private BallDescriptionUi _descUi;

        private int _currentTryCount = 0;
        private int _thisTurnTryCount = 2;

        public void Initialize()
        {
            _bus.OnInit += InitShellGame;
            _battleBus.OnShellGameStarted += OnShellGameStarted;
            _buttonUi.OnBallChosen += OnPlayerBallCountChoose;
            _buttonUi.StartShuffleButton.onClick.AddListener(Shuffle);
            _view.OnCupClicked += OnCupClicked;
            _roomRewardBus.OnRoomCompleted += CleanUp;

            _view.OnEnter += ShowDesc;
            _view.OnExit += _descUi.Hide;
        }

        public void Dispose()
        {
            _bus.OnInit -= InitShellGame;
            _battleBus.OnShellGameStarted -= OnShellGameStarted;
            _buttonUi.OnBallChosen -= OnPlayerBallCountChoose;
            _buttonUi.StartShuffleButton.onClick.RemoveListener(Shuffle);
            _view.OnCupClicked -= OnCupClicked;
            _roomRewardBus.OnRoomCompleted -= CleanUp;
            _view.OnEnter -= ShowDesc;
            _view.OnExit -= _descUi.Hide;
        }

        private void ShowDesc(string obj)
        {
            _descUi.SetDesc(obj);
            _descUi.Show();
        }

        private void CleanUp()
        {
            _view.FinishBattleCleanUp();
        }

        private void OnPlayerBallCountChoose(int ballCount)
        {
            Debug.Log($" On player ball count choose: {ballCount}");
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
            Debug.Log($"On Cup clicked: {cupView.BallView?.UnitType}");
            if (_currentTryCount < _thisTurnTryCount)
            {
                _currentTryCount++;
                cupView.ShowBall();

                var coll = cupView.GetComponent<CapsuleCollider2D>();
                coll.enabled = false;

                if (cupView.BallView == null || cupView.BallView.UnitType == BallUnitType.None)
                {
                    EndTurn();
                    return;
                }
                else if (cupView.BallView.UnitType == BallUnitType.Enemy)
                    BoostEnemy(cupView);
                else
                {
                    _clownEventBus.UserChoseCupSuccess();
                    _battleSystem.ChoosePlayerBall(cupView.BallView.BallId);
                }

                if (_currentTryCount >= _thisTurnTryCount)
                    EndTurn();
            }
            else
                Debug.LogError("а схренали мы кликаем за пределами каунта?");
        }

        private void BoostEnemy(CupView cupView)
        {
            _clownEventBus.UserChoseCupFail();
            Debug.Log($" вытащили шарик врага, бустим врага (пока нет)");
            _battleSystem.ChooseEnemyBall();
        }

        private async void EndTurn()
        {
            _buttonUi.HideChooseButtonInteraction();
            _view.ShowBallsForAllCups();
            _view.Unsubscribe();
            await UniTask.Delay(500);
            _battleSystem.StartPlayerTurn();
        }
    }
}