using System.Collections.Generic;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RewardUiData
    {
        public RewardUiData(List<RewardCardUiData> rewards) => Rewards = rewards;
        public List<RewardCardUiData> Rewards { get; set; }
    }
}