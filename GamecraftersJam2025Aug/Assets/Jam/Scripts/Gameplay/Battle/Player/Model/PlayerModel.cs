using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerModel : BaseUnit
    {
        public bool IsActive { get; private set; }
        // public List<PlayerBallModel> Balls { get; set; } = new();
        public List<int> CurrentBalls { get; set; } = new();
        public PlayerModel(int health)
        {
            Health = health;
            MaxHealth = health;
            IsDead = false;
        }
        
        public bool SetActive(bool isActive) => IsActive = isActive;
        // public void AddBall(PlayerBallModel ball) => Balls.Add(ball);
        public void ClearBalls()
        {
            // Balls.Clear();
            CurrentBalls.Clear();
        }

        public void AddBallId(int ballId)
        {
            CurrentBalls.Add(ballId);
        }
    }
}