using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class PlayerModel : BaseUnit
    {
        public bool IsActive { get; private set; }
        public List<int> CurrentBallIds { get; set; } = new();
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
        }

        public void AddBallId(int ballId)
        {
            CurrentBallIds.Add(ballId);
        }
    }
}