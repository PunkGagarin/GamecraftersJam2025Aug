using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RandomBallRewardCardUiData : IRewardCardUiData
    {
        public Sprite Icon { get; set; }
        public string Desc { get; set; }

        public RewardBallFieldState SelectedBall { get; set; }
        public List<BallRewardCardUiData> RewardCards { get; set; }
        public RandomBallRewardCardUiData(List<BallRewardCardUiData> rewardCards) => RewardCards = rewardCards;
    }
}