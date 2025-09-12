using Jam.Scripts.Gameplay.Inventory.Models;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RewardBallFieldState
    {
        public IRewardCardUiData RewardCardUiData { get; set; }
        public BallType BallType { get; set; }
        public int Grade { get; set; }

        public RewardBallFieldState(IRewardCardUiData rewardCardUiData, BallType ballType, int grade)
        {
            RewardCardUiData = rewardCardUiData;
            BallType = ballType;
            Grade = grade;
        }
    }
}