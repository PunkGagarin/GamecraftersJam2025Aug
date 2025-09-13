using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class PlayerModel : BaseUnit
    {
        public bool IsActive { get; private set; }
        public List<int> CurrentBallIds { get; set; } = new();
        public List<BallDto> CurrentBalls { get; set; } = new();
        public PlayerModel(int health)
        {
            Health = health;
            MaxHealth = health;
            IsDead = false;
        }
        
        public bool SetActive(bool isActive) => IsActive = isActive;
        public void ClearBalls()
        {
            // Balls.Clear();
            CurrentBallIds.Clear();
            CurrentBalls.Clear();
        }

        public void AddBallId(int ballId)
        {
            CurrentBallIds.Add(ballId);
        }

        public void AddBallDto(BallDto ball)
        {
            CurrentBalls.Add(ball);
        }
    }
}