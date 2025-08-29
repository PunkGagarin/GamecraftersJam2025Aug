namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class PlayerBallModel
    {
        private int BallId { get; set; }
        public TargetType TargetType { get; set; }

        public PlayerBallModel(int ballId, int damage, TargetType targetType)
        {
            Damage = damage;
            BallId = ballId;
            TargetType = targetType;
        }

        public int Damage { get; private set; }
    }

}