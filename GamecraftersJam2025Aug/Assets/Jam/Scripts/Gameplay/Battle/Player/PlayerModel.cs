namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerModel : BaseUnit
    {
        public bool IsActive { get; private set; }
        public PlayerModel(int health)
        {
            Health = health;
            MaxHealth = health;
            IsDead = false;
        }
        
        public bool SetActive(bool isActive) => IsActive = isActive;
    }
}