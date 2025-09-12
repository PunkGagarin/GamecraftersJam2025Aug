using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Systems
{
    public class WinDto
    {
        public List<BallRewardCardUiData> Balls { get; set; } = new();
        public int HealAmount { get; set; }
        public int HealCost { get; set; }
    }
}