using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameManagerView : MonoBehaviour
    {
        [Inject] private readonly ShellGameButtonUi _buttonUi;

        [field: SerializeField]
        public Transform StartPosition { get; private set; }

        [field: SerializeField]
        public Button ShuffleButton { get; private set; }

        public int MinShuffleCount { get; private set; }
        public int MaxShuffleCount { get; private set; }
        public float NextPairPickupSpeed { get; private set; }
        public float CupShuffleSpeed { get; private set; }


        private List<CupView> _cups;
        private List<BallView> _myBalls;
        private (CupView one, CupView two) _currentPair;

        public event Action<CupView> OnCupClicked = delegate { };

        public void Init(ShellGameConfig shellGameConfig)
        {
            ParseConfig(shellGameConfig);
            Create(shellGameConfig);
            PlaceAllBallsToRandomCup();
            ShowBallsForAllCups();

            Subscribe();
        }

        private void ParseConfig(ShellGameConfig shellGameConfig)
        {
            MinShuffleCount = shellGameConfig.MinShuffleCount;
            MaxShuffleCount = shellGameConfig.MaxShuffleCount;
            NextPairPickupSpeed = shellGameConfig.NextPairPickupSpeed;
            CupShuffleSpeed = shellGameConfig.CupShuffleSpeed;
        }

        private void Subscribe()
        {
            foreach (var cup in _cups)
                cup.OnClicked += OnCupClicked;
        }

        private void Create(ShellGameConfig shellGameConfig)
        {
            _cups = new List<CupView>();
            _myBalls = new List<BallView>();
            for (int i = 0; i < shellGameConfig.CupCount; i++)
            {
                var cupView = Instantiate(shellGameConfig.CupViewPrefab, Vector3.zero, Quaternion.identity);
                cupView.transform.position = StartPosition.position;
                cupView.transform.localPosition += new Vector3((i % 3) * 2.5f, (i / 3) * 2f * -1, 0);

                _cups.Add(cupView);
            }

            for (int i = 0; i < shellGameConfig.GreenBallCount; i++)
            {
                var greenBall = Instantiate(shellGameConfig.GreenBallViewPrefab, Vector3.zero, Quaternion.identity);
                _myBalls.Add(greenBall);
            }
            for (int i = 0; i < shellGameConfig.RedBallCount; i++)
            {
                var redBall = Instantiate(shellGameConfig.RedBallViewPrefab, Vector3.zero, Quaternion.identity);
                _myBalls.Add(redBall);
            }
        }

        private void PlaceAllBallsToRandomCup()
        {
            List<CupView> choosenCups = new List<CupView>();

            for (int i = 0; i < _myBalls.Count; i++)
            {
                var cup = _cups[Random.Range(0, _cups.Count)];
                while (choosenCups.Contains(cup))
                {
                    cup = _cups[Random.Range(0, _cups.Count)];
                }

                cup.SetBall(_myBalls[i]);
                choosenCups.Add(cup);
            }
        }

        public async void Shuffle()
        {
            HideBallsForAllCups();
            MakeAllThiblesUninteractable();

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
            {
                Cup.HideBall();
            }
        }

        public void ShowBallsForAllCups()
        {
            foreach (var Cup in _cups)
            {
                Cup.ShowBall();
            }
        }

        private void MakeAllThiblesUninteractable()
        {
            foreach (var Cup in _cups)
            {
                var collider = Cup.GetComponent<CapsuleCollider2D>();
                collider.enabled = false;
            }
        }

        private void PickPair()
        {
            var firstCup = _cups[Random.Range(0, _cups.Count)];
            var secondCup = _cups[Random.Range(0, _cups.Count)];

            while (firstCup == secondCup)
                secondCup = _cups[Random.Range(0, _cups.Count)];

            _currentPair = (firstCup, secondCup);
        }

        private async Task ShufflePair()
        {
            // Debug.LogError("shuffling pair");

            MoveUtils.MoveOverTime2D(_currentPair.one.transform, _currentPair.two.transform.position,
                CupShuffleSpeed);
            await MoveUtils.MoveOverTime2D(_currentPair.two.transform, _currentPair.one.transform.position,
                CupShuffleSpeed);
        }

        private void MakeAllCupsInteractable()
        {
            foreach (var Cup in _cups)
            {
                var collider = Cup.GetComponent<CapsuleCollider2D>();
                collider.enabled = true;
            }
        }
    }
}