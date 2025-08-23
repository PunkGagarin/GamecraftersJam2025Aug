 namespace Jam.Scripts.Gameplay.Battle
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