using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Queue
{
    public class BattleQueueService
    {
        [Inject] private BattleQueueBus _bus;

        private Queue<PlayerBallModel> _ballsQueue;
        private List<PlayerBallModel> _usedBalls;

        public void Init(List<PlayerBallModel> balls)
        {
            _ballsQueue = new Queue<PlayerBallModel>(balls);
            _usedBalls = new();

            var ballDtos = balls.Select(b => new BallDto(b.BallId, b.Sprite, b.Description)).ToList();
            _bus.Init(ballDtos);
            _ballsQueue.Shuffle();

            List<int> ballIds = _ballsQueue.Select(b => b.BallId).ToList();
            _bus.BallsShuffled(ballIds);
        }

        public List<BallDto> GetNextBall(int ballCount)
        {
            List<PlayerBallModel> nextBalls = new();

            if (_ballsQueue.Count == 0)
            {
                Debug.LogError("Trying to get ball but the queue is empty");
                return null;
            }

            for (int i = 0; i < ballCount; i++)
            {
                if (_ballsQueue.Count == 0)
                {
                    Debug.Log("Skip turn feature should be here??");
                    break;
                }
                else
                {
                    var nextBall = _ballsQueue.Dequeue();
                    nextBalls.Add(nextBall);
                    _usedBalls.Add(nextBall);
                }
            }

            _bus.NextBallsChoosen(ConvertBallsToIds(nextBalls));

            if (_ballsQueue.Count == 0)
                ShuffleUsedBalls();

            return ConvertBallModelToDtos(nextBalls);
        }

        public void ShuffleUsedBalls()
        {
            _usedBalls.Shuffle();

            foreach (var ball in _usedBalls)
                _ballsQueue.Enqueue(ball);

            List<int> ballIds = _usedBalls.Select(b => b.BallId).ToList();
            _bus.BallsShuffled(ballIds);

            _usedBalls.Clear();
        }

        private List<BallDto> ConvertBallModelToDtos(List<PlayerBallModel> balls)
        {
            return balls.Select(b => new BallDto(b.BallId, b.Sprite, b.Description)).ToList();
        }

        private List<int> ConvertBallsToIds(List<PlayerBallModel> balls)
        {
            return balls.Select(b => b.BallId).ToList();
        }

        public int GetQueueCount()
        {
            return _ballsQueue.Count;
        }
    }
}