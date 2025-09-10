using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class ConcreteBallRewardCardUiData : IRewardCardUiData
    {
        public ConcreteBallRewardCardUiData(BallRewardCardUiData ballReward) => BallReward = ballReward;
        public BallRewardCardUiData BallReward { set; get; }
        
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