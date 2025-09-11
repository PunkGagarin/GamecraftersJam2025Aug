using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallUpgradeRewardCardUiData : IRewardCardUiData
    {
        public BallUpgradeRewardCardUiData(Sprite icon, string desc)
        {
            Icon = icon;
            Desc = desc;
        }
        public Sprite Icon { get; set; }
        public string Desc { get; set; }
    }
}