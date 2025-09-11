using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallRewardCardUiData: IRewardCardUiData
    {
        public BallRewardCardUiData(Sprite icon, string desc, BallType type, int grade)
        {
            Icon = icon;
            Desc = desc;
            Type = type;
            Grade = grade;
        }
        public Sprite Icon { get; set; }
        public int Grade { get; set; }
        public string Desc { get; set; }
        public BallType Type { get; set; }
    }
}