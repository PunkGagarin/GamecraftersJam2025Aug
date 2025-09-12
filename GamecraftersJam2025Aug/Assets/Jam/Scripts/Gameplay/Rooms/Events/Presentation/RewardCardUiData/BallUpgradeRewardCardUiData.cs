using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallUpgradeRewardCardUiData : IRewardCardUiData
    {
        public BallUpgradeRewardCardUiData(BallRewardCardUiData prevBall, BallRewardCardUiData newBall, Sprite icon,
            string desc)
        {
            PrevBall = prevBall;
            NewBall = newBall;
            Icon = icon;
            Desc = desc;
        }

        public BallRewardCardUiData PrevBall { get; set; }
        public BallRewardCardUiData NewBall { get; set; }
        public Sprite Icon { get; set; }
        public string Desc { get; set; }
    }
}