using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameView : MonoBehaviour
    {
        [Inject] private readonly ShellGameButtonUi _buttonUi;
        [Inject] private readonly ShellGameShuffler _shuffler;
        [Inject] private readonly ShellCreator _shellCreator;

        [field: SerializeField]
        public Transform StartPosition { get; private set; }


        private List<CupView> _cups;
        private List<BoardBallView> _balls;
        private int _startedCupOrder;
        private bool _isShuffling = false;

        private List<CupView> ActiveCups => _cups.FindAll(c => c.gameObject.activeSelf);

        public event Action<CupView> OnCupClicked = delegate { };

        private void Update()
        {
            if (_isShuffling)
                SetOrdersForCurrentCups();
        }

        public void Init(ShellGameConfig shellGameConfig)
        {
            ParseConfig(shellGameConfig);
            _shuffler.Init(shellGameConfig);

            _cups = new List<CupView>();
            _balls = new List<BoardBallView>();
        }


        private void Subscribe()
        {
            foreach (var cup in _cups)
                cup.OnClicked += OnCupClickedInvoke;
        }

        public void Unsubscribe()
        {
            foreach (var cup in _cups)
                cup.OnClicked -= OnCupClickedInvoke;
        }

        public void OnCupClickedInvoke(CupView cup)
        {
            OnCupClicked.Invoke(cup);
        }

        public async void Shuffle()
        {
            _isShuffling = true;
            Subscribe();
            HideBallsForAllCups();
            MakeAllCupsUninteractable();
            await _shuffler.Shuffle(ActiveCups);
            MakeAllCupsInteractable();
            _isShuffling = false;
        }

        private void SetOrdersForCurrentCups()
        {
            _cups.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));

            for (int i = 0; i < _cups.Count; i++)
                _cups[i].GetComponent<SpriteRenderer>().sortingOrder = _startedCupOrder + i;
        }

        private void HideBallsForAllCups()
        {
            foreach (var cup in _cups)
                cup.HideBall();
        }

        private void MakeAllCupsUninteractable()
        {
            foreach (var cup in _cups)
            {
                var coll = cup.GetComponent<CapsuleCollider2D>();
                coll.enabled = false;
            }
        }

        private void MakeAllCupsInteractable()
        {
            foreach (var cup in _cups)
            {
                var coll = cup.GetComponent<CapsuleCollider2D>();
                coll.enabled = true;
            }
        }

        public void InitRoundBalls(List<BallDto> balls)
        {
            List<BoardBallView> listBallViews = _balls.FindAll(b => b.UnitType == BallUnitType.Player);

            if (balls.Count != listBallViews.Count)
            {
                Debug.LogError("количество вьюшек и выбранных шаров НЕ СОВПАДАЕТ!");
                return;
            }

            for (int i = 0; i < balls.Count; i++)
            {
                var ballView = listBallViews[i];
                ballView.Init(balls[i]);
            }
        }

        private void ParseConfig(ShellGameConfig shellGameConfig)
        {
            _shellCreator.ParseConfig(shellGameConfig);
            _startedCupOrder = shellGameConfig.CupViewPrefab.GetComponent<SpriteRenderer>().sortingOrder;
        }

        public void PrepareForShuffle(int ballsCount, List<BallDto> ballIds, ShellGameCupAndBallInfo config)
        {
            _shellCreator.CreateNeededObjects(ballsCount, config, _cups, _balls);

            InitPlayerBalls(_balls, ballIds);
            SetCupsPosition();
            PlaceAllBallsToRandomCup();
            ShowBallsForAllCups();
        }

        private void InitPlayerBalls(List<BoardBallView> _balls, List<BallDto> currentBalls)
        {
            var activeBalls = _balls.FindAll(b => b.gameObject.activeSelf && b.UnitType == BallUnitType.Player);
            if (activeBalls.Count != currentBalls.Count)
                Debug.LogError("something is wrong with balls");

            for (int i = 0; i < activeBalls.Count; i++)
            {
                var view = activeBalls[i];
                var ballDto = currentBalls[i];
                view.Init(ballDto);
            }
        }

        private void SetCupsPosition()
        {
            var activeCups = _cups.FindAll(c => c.gameObject.activeSelf);
            for (int i = 0; i < activeCups.Count; i++)
            {
                var cupView = activeCups[i];
                cupView.transform.position = StartPosition.position + FindCupOffset(i);
            }
        }

        private Vector3 FindCupOffset(int i)
        {
            float xOffsetPerCup = 2.5f;
            float yOffsetPerThreeCup = 2f * -1f;

            return new Vector3((i % 3) * xOffsetPerCup, (i / 3) * yOffsetPerThreeCup, 0);
        }

        private void PlaceAllBallsToRandomCup()
        {
            List<CupView> choosenCups = new List<CupView>();
            var activeCups = _cups.FindAll(c => c.gameObject.activeSelf);
            var activeBalls = _balls.FindAll(b => b.gameObject.activeSelf);

            for (int i = 0; i < activeBalls.Count; i++)
            {
                var cup = activeCups[Random.Range(0, activeCups.Count)];
                while (choosenCups.Contains(cup))
                {
                    cup = activeCups[Random.Range(0, activeCups.Count)];
                }

                cup.SetBall(activeBalls[i]);
                choosenCups.Add(cup);
            }
            foreach (var cup in activeCups)
            {
                if (!choosenCups.Contains(cup))
                    cup.RemoveBall();
            }
        }

        public void ShowBallsForAllCups()
        {
            foreach (var Cup in _cups)
                Cup.ShowBall();
        }

        public void CleanUp()
        {
            foreach (CupView cup in _cups)
                cup.RemoveBall();

            foreach (BoardBallView ball in _balls)
                ball.gameObject.SetActive(false);
        }

        public void FinishBattleCleanUp()
        {
            foreach (CupView cup in _cups)
                Destroy(cup.gameObject);

            foreach (BoardBallView ball in _balls)
                Destroy(ball.gameObject);

            _cups.Clear();
            _balls.Clear();
        }
    }
}