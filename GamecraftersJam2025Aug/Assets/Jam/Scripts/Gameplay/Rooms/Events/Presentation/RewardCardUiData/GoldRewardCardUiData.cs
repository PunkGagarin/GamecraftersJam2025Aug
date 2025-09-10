using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    class GoldRewardCardUiData : IRewardCardUiData
    {
        public GoldRewardCardUiData(Sprite icon, string desc, float value)
        {
            Icon = icon;
            Desc = desc;
            Value = value;
        }
        public Sprite Icon { get; set; }
        public string Desc { get; set; }
        public float Value { get; set; }
    }
}