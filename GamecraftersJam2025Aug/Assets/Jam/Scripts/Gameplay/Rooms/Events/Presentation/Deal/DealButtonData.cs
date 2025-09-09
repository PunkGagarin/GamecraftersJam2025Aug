namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class DealButtonData
    {
        public DealButtonData(string description, RewardCardUiData reward, RewardCardUiData risk)
        {
            Description = description;
            Reward = reward;
            Risk = risk;
        }
        public string Description { get; set; }
        public RewardCardUiData Reward { get; set; }
        public RewardCardUiData Risk { get; set; }
    }
}