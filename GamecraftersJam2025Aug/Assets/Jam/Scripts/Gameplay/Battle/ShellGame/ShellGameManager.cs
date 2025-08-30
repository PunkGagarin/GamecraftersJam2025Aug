using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameManager : MonoBehaviour
    {

        [field: SerializeField]
        public Thimble ThimblePrefab { get; private set; }

        [field: SerializeField]
        public Transform StartPosition { get; private set; }

        [field: SerializeField]
        public MyBall GreenBallPrefab { get; private set; }

        [field: SerializeField]
        public MyBall RedBallPrefab { get; private set; }

        [field: SerializeField]
        public int ThimbleCount { get; private set; }

        [field: SerializeField]
        public int GreenBallCount { get; private set; }

        [field: SerializeField]
        public int RedBallCount { get; private set; }

        [field: SerializeField]
        public int MinShuffleCount { get; private set; }

        [field: SerializeField]
        public int MaxShuffleCount { get; private set; }

        [field: SerializeField]
        public int TryCount { get; private set; }

        [field: SerializeField]
        public float NextPairPickupSpeed { get; private set; }

        [field: SerializeField]
        public float ThimbleShuffleSpeed { get; private set; }

        [field: SerializeField]
        public Button ShuffleButton { get; private set; }


        private List<Thimble> _thimbles;
        private List<MyBall> _myBalls;
        private (Thimble one, Thimble two) _currentPair;
        private int _currentTryCount = 0;

        [Inject] private readonly ShellGameView _buttonView;

        public void Initialize()
        {
            Init();

            foreach (var thimble in _thimbles)
                thimble.OnClicked += OnThimbleClicked;

            ShowBallsForAllThimbles();
            
            _buttonView.ChooseAttackButton.onClick.AddListener(Shuffle);
            _buttonView.ChooseAttackButton.interactable = true;
        }

        private void Init()
        {
            //instantiate thimbles by thimble count, 3 thimbles per raw, x step = 4.5, y step = 4.5
            _thimbles = new List<Thimble>();
            _myBalls = new List<MyBall>();
            for (int i = 0; i < ThimbleCount; i++)
            {
                var thimble = Instantiate(ThimblePrefab, Vector3.zero, Quaternion.identity);
                // thimble.MadeColorRandom();
                //start from start position
                thimble.transform.position = StartPosition.position;
                thimble.transform.localPosition += new Vector3((i % 3) * 4.5f, (i / 3) * 4.5f * -1, 0);

                _thimbles.Add(thimble);
            }

            //create green and red balls
            for (int i = 0; i < GreenBallCount; i++)
            {
                var greenBall = Instantiate(GreenBallPrefab, Vector3.zero, Quaternion.identity);
                _myBalls.Add(greenBall);
            }
            for (int i = 0; i < RedBallCount; i++)
            {
                var redBall = Instantiate(RedBallPrefab, Vector3.zero, Quaternion.identity);
                _myBalls.Add(redBall);
            }

            PlaceAllBallsToRandomThimbles();
        }

        private void PlaceAllBallsToRandomThimbles()
        {
            List<Thimble> choosenThimbles = new List<Thimble>();

            for (int i = 0; i < _myBalls.Count; i++)
            {
                var thimble = _thimbles[Random.Range(0, _thimbles.Count)];
                while (choosenThimbles.Contains(thimble))
                {
                    thimble = _thimbles[Random.Range(0, _thimbles.Count)];
                }

                thimble.SetBall(_myBalls[i]);
                choosenThimbles.Add(thimble);
            }
        }

        private async void OnThimbleClicked(Thimble thimble)
        {
            if (_currentTryCount < TryCount)
            {
                _currentTryCount++;

                if (thimble.MyBall == null || thimble.MyBall.Type == MyBallType.None)
                {
                    ShowBallsForAllThimbles();
                }
                thimble.ShowBall();
                if (_currentTryCount >= TryCount)
                {
                    await Task.Delay(500);
                    ShowBallsForAllThimbles();
                }
            }
            else
            {
                Debug.LogError("а схренали мы кликаем за пределами каунта?");
            }
        }

        public async void Shuffle()
        {
            ShuffleButton.interactable = false;
            
            _currentTryCount = 0;

            HideBallsForAllThimbles();
            MakeAllThiblesUninteractable();

            for (int i = 0; i < Random.Range(MinShuffleCount, MaxShuffleCount); i++)
            {
                PickPair();
                await ShufflePair();
                await Task.Delay((int)(NextPairPickupSpeed * 1000));
            }

            MakeAllThiblesInteractable();
            ShuffleButton.interactable = true;
        }

        private void HideBallsForAllThimbles()
        {
            // Debug.LogError("Hiding all balls");
            foreach (var thimble in _thimbles)
            {
                thimble.HideBall();
            }
        }

        private void ShowBallsForAllThimbles()
        {
            // Debug.LogError("Hiding all balls");
            foreach (var thimble in _thimbles)
            {
                thimble.ShowBall();
            }
        }


        private void MakeAllThiblesUninteractable()
        {
            // Debug.LogError("Making all thibles uninteractable");
            foreach (var thimble in _thimbles)
            {
                var collider = thimble.GetComponent<CapsuleCollider2D>();
                collider.enabled = false;
            }
        }

        private void PickPair()
        {
            // Debug.LogError("picking up pair");

            var firstThimble = _thimbles[Random.Range(0, _thimbles.Count)];
            var secondThimble = _thimbles[Random.Range(0, _thimbles.Count)];

            while (firstThimble == secondThimble)
                secondThimble = _thimbles[Random.Range(0, _thimbles.Count)];

            _currentPair = (firstThimble, secondThimble);
        }

        private async Task ShufflePair()
        {
            // Debug.LogError("shuffling pair");

            MoveUtils.MoveOverTime2D(_currentPair.one.transform, _currentPair.two.transform.position,
                ThimbleShuffleSpeed);
            await MoveUtils.MoveOverTime2D(_currentPair.two.transform, _currentPair.one.transform.position,
                ThimbleShuffleSpeed);
        }

        private void MakeAllThiblesInteractable()
        {
            // Debug.LogError("Making all thibles interactable");
            foreach (var thimble in _thimbles)
            {
                var collider = thimble.GetComponent<CapsuleCollider2D>();
                collider.enabled = true;
            }
        }
    }
}