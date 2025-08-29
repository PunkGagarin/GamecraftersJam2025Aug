using System;
using System.Collections.Generic;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class BallsInventoryModel
    {
        public List<PlayerBallModel> Balls { get; private set; } = new();

        public event Action<List<PlayerBallModel>, PlayerBallModel> OnBallsChanged = delegate { };

        public void Init(List<PlayerBallModel> balls)
        {
            Balls.AddRange(balls);
        }

        public void AddBall(PlayerBallModel ball)
        {
            Balls.Add(ball);
            OnBallsChanged.Invoke(Balls, ball);
        }

        public void RemoveBall(PlayerBallModel ball)
        {
            Balls.Remove(ball);
            OnBallsChanged.Invoke(Balls, ball);
        }

        public void UpdateBall(PlayerBallModel oldBall, PlayerBallModel upgradedBall)
        {
            Balls.Remove(oldBall);
            Balls.Add(upgradedBall);
            OnBallsChanged.Invoke(Balls, upgradedBall);
        }
        
        
    }
}