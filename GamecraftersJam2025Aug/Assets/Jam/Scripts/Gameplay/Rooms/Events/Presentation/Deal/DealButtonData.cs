namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class DealButtonData
    {
        public DealButtonData(string description, IRewardCardUiData reward, IRiskCardUiData risk)
        {
            Description = description;
            Reward = reward;
            Risk = risk;
        }
        public string Description { get; set; }
        public IRewardCardUiData Reward { get; set; }
        public IRiskCardUiData Risk { get; set; }
    }
}