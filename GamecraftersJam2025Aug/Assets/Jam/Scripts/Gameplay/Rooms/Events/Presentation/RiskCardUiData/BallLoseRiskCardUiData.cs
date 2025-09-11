namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class BallLoseRiskCardUiData : IRiskCardUiData
    {
        public BallRewardCardUiData BallReward { set; get; }
        public BallLoseRiskCardUiData(BallRewardCardUiData ballReward) => BallReward = ballReward;
        public string Desc { get; set; }
    }
}