using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RewardUiData
    {
        public Sprite Icon { get; set; }

        public List<IRewardCardUiData> Rewards { get; set; }

        public RewardUiData(Sprite icon, List<IRewardCardUiData> rewards)
        {
            Icon = icon;
            Rewards = rewards;
        }
    }
}