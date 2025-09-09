using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RewardCardUiData
    {
        public RewardCardUiData(Sprite icon, string descKey)
        {
            Icon = icon;
            DescKey = descKey;
        }
        public Sprite Icon { get; set; }
        public string DescKey { get; set; }
    }
}