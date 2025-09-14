using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class BallBattleDto
    {
        public List<EffectInstance> Effects { get; set; }
        public BallType Type { get; set; }

        public BallBattleDto(PlayerBallModel ballModel)
        {
            Effects = ballModel.Effects;
            Type = ballModel.Type;
        }

        public BallBattleDto(List<EffectInstance> effects)
        {
            Effects = effects;
        }
    }
}