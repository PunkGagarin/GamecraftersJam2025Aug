using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameView : MonoBehaviour
    {
        [Inject] private readonly ShellGameButtonUi _buttonUi;

        [field: SerializeField]
        public Transform StartPosition { get; private set; }

        public int MinShuffleCount { get; private set; }
        public int MaxShuffleCount { get; private set; }
        public float NextPairPickupSpeed { get; private set; }
        public float CupShuffleSpeed { get; private set; }


        private CupView _cupViewPrefab;
        private BoardBallView _greenBallViewPrefab;
        private BoardBallView _redBallViewPrefab;

        private List<CupView> _cups;
        private List<BoardBallView> _balls;
        private (CupView one, CupView two) _currentPair;

        private List<CupView> ActiveCups => _cups.FindAll(c => c.gameObject.activeSelf);

        public event Action<CupView> OnCupClicked = delegate { };

        public void Init(ShellGameConfig shellGameConfig)
        {
            ParseConfig(shellGameConfig);

            _cups = new List<CupView>();
            _balls = new List<BoardBallView>();
        }

        private void ParseConfig(ShellGameConfig shellGameConfig)
        {
            _cupViewPrefab = shellGameConfig.CupViewPrefab;
            _greenBallViewPrefab = shellGameConfig.GreenBallViewPrefab;
            _redBallViewPrefab = shellGameConfig.RedBallViewPrefab;

            MinShuffleCount = shellGameConfig.MinShuffleCount;
            MaxShuffleCount = shellGameConfig.MaxShuffleCount;
            NextPairPickupSpeed = shellGameConfig.NextPairPickupSpeed;
            CupShuffleSpeed = shellGameConfig.CupShuffleSpeed;
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
            Subscribe();
            HideBallsForAllCups();
            MakeAllCupsUninteractable();

            for (int i = 0; i < Random.Range(MinShuffleCount, MaxShuffleCount); i++)
            {
                PickPair();
                await ShufflePair();
                await Task.Delay((int)(NextPairPickupSpeed * 1000));
            }
            MakeAllCupsInteractable();
        }

        private void HideBallsForAllCups()
        {
            foreach (var Cup in _cups)
                Cup.HideBall();
        }

        private void MakeAllCupsUninteractable()
        {
            foreach (var cup in _cups)
            {
                var coll = cup.GetComponent<CapsuleCollider2D>();
                coll.enabled = false;
            }
        }

        private void PickPair()
        {
            var firstCup = ActiveCups[Random.Range(0, ActiveCups.Count)];
            var secondCup = ActiveCups[Random.Range(0, ActiveCups.Count)];

            while (firstCup == secondCup)
                secondCup = ActiveCups[Random.Range(0, ActiveCups.Count)];

            _currentPair = (firstCup, secondCup);
        }

        private async Task ShufflePair()
        {
            MoveUtils.MoveOverTime2D(_currentPair.one.transform, _currentPair.two.transform.position,
                CupShuffleSpeed);
            await MoveUtils.MoveOverTime2D(_currentPair.two.transform, _currentPair.one.transform.position,
                CupShuffleSpeed);
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

        public void PrepareForShuffle(int ballsCount, List<BallDto> ballIds, ShellGameCupAndBallInfo config)
        {
            TurnOnOrCreate(config.CupCount, _cups, _cups, _cupViewPrefab);

            var playerBalls = _balls.FindAll(b => b.UnitType == BallUnitType.Player);
            TurnOnOrCreate(ballsCount, _balls, playerBalls, _greenBallViewPrefab);

            var enemyBalls = _balls.FindAll(b => b.UnitType == BallUnitType.Enemy);
            TurnOnOrCreate(config.RedBallCount, _balls, enemyBalls, _redBallViewPrefab);

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
                cupView.transform.position = StartPosition.position;
                cupView.transform.localPosition += new Vector3((i % 3) * 2.5f, (i / 3) * 2f * -1, 0);
            }
        }

        private void TurnOnOrCreate<T>(int count, List<T> sharedList, List<T> specList, T prefab)
            where T : MonoBehaviour
        {
            if (specList.Count < count)
                CreateAndAddInstance(count - specList.Count, prefab, sharedList);

            for (int i = 0; i < specList.Count; i++)
            {
                var ball = specList[i];
                ball.gameObject.SetActive(i < count);
            }
        }

        private void CreateAndAddInstance<T>(int maxCount, T viewPrefab, List<T> listToAdd)
            where T : MonoBehaviour
        {
            for (int i = 0; i < maxCount; i++)
            {
                var cupView = Instantiate(viewPrefab, Vector3.zero, Quaternion.identity);
                listToAdd.Add(cupView);
            }
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
            {
                cup.RemoveBall();
            }

            foreach (BoardBallView ball in _balls)
            {
                ball.gameObject.SetActive(false);
            }
        }

        public void FinishBattleCleanUp()
        {
            foreach (CupView cup in _cups)
            {
                Destroy(cup.gameObject);
            }

            foreach (BoardBallView ball in _balls)
            {
                Destroy(ball.gameObject);
            }
            _cups.Clear();
            _balls.Clear();
        }
    }
}