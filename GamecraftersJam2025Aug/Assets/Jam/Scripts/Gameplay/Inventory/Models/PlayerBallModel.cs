 namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class PlayerBallModel
    {
        public PlayerBallModel(int damage)
        {
            Damage = damage;
        }

        public int Damage { get; private set; }
    }
}