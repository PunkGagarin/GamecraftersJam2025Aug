using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallLoseRiskCardUiData : IRiskCardUiData
    {
        public BallRewardCardUiData BallReward { set; get; }
        public BallLoseRiskCardUiData(BallRewardCardUiData ballReward) => BallReward = ballReward;

        public Sprite Icon
        {
            get => BallReward.Icon;
            set { }
        }

        public string Desc
        {
            get => BallReward.Desc;
            set { }
        }
    }
}