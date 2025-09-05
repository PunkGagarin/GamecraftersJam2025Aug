using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class BallBattleDto
    {
        public int Damage { get; set; }
        public TargetType TargetType { get; set; }
        public List<EffectInstance> Effects { get; set; }

        public BallBattleDto(int damage, TargetType targetType, List<EffectInstance> effects)
        {
            Damage = damage;
            TargetType = targetType;
            Effects = effects;
        }
    }
}