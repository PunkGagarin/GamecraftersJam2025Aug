using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleQueuePresenter
    {
        //balls queue
        //artifacts
        [Inject] private BallsInventoryModel _ballsInventoryModel;
        [Inject] private BattleBallsQueueView _queueView;

        private Queue<PlayerBallModel> _ballsQueue = new();
        //когда очередь пустеет что приосходит?

        public void InitBattleData()
        {
            _ballsQueue = new Queue<PlayerBallModel>(_ballsInventoryModel.Balls);
            _ballsQueue.Shuffle();
            //map model to view
        }

        public PlayerBallModel GetNextBall()
        {
            if (_ballsQueue.Count == 0)
                return null;

            var nextBall = _ballsQueue.Dequeue();
            // _queueView.AddBallToQueue(nextBall);

            if (_ballsQueue.Count == 0)
            {
                Debug.LogError("balls queue is empty we have no next ball");
            }
            return nextBall;
        }
    }

    public class BattleBallsQueueView
    {
        public List<PlayerBallsView> BallsViews { get; set; } = new();
    }

    public class PlayerBallsView
    {
        
    }
}