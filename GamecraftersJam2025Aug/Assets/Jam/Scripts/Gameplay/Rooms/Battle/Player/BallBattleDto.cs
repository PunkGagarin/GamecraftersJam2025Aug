using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class BallBattleDto
    {
        public int Damage { get; set; }
        public TargetType TargetType { get; set; }

        public BallBattleDto(int damage, TargetType targetType)
        {
            Damage = damage;
            TargetType = targetType;
        }
    }
}