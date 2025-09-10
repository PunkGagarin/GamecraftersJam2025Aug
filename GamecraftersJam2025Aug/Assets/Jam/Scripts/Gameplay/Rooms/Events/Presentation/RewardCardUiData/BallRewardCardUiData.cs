using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallRewardCardUiData: IRewardCardUiData
    {
        public BallRewardCardUiData(Sprite icon, string desc, BallType type)
        {
            Icon = icon;
            Desc = desc;
            Type = type;
        }
        public Sprite Icon { get; set; }
        public string Desc { get; set; }
        public BallType Type { get; set; }
    }
}