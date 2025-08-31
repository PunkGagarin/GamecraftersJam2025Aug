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
        }

        public List<int> GetNextBall(int ballCount)
        {
            List<int> nextBalls = new();

            if (_ballsQueue.Count == 0)
            {
                Debug.LogError("Trying to get ball but the queue is empty");
                return null;
            }

            for (int i = 0; i < ballCount; i++)
            {
                if (_ballsQueue.Count == 0)
                {
                    Debug.LogError("balls queue is empty we have no next ball, do smth with it");
                    ShuffleUsedBalls();
                    break;
                }

                var nextBall = _ballsQueue.Dequeue();
                nextBalls.Add(nextBall.BallId);
                _usedBalls.Add(nextBall);
            }

            _bus.NextBallsChoosen(nextBalls);

            if (_ballsQueue.Count == 0)
                ShuffleUsedBalls();

            return nextBalls;
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
    }
}