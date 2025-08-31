using System;
using System.Collections.Generic;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class BallsInventoryModel
    {
        public List<PlayerBallModel> Balls { get; private set; } = new();

        public void AddBall(PlayerBallModel ball)
        {
            Balls.Add(ball);
        }

        public void RemoveBall(PlayerBallModel ball)
        {
            Balls.Remove(ball);
        }

        public void UpdateBall(PlayerBallModel oldBall, PlayerBallModel upgradedBall)
        {
            Balls.Remove(oldBall);
            Balls.Add(upgradedBall);
        }

        public void AddBalls(List<PlayerBallModel> balls)
        {
            Balls.AddRange(balls);
        }
    }
}