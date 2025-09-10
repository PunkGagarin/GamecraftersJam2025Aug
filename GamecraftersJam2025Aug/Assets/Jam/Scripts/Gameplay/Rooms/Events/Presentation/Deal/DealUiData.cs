using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class DealUiData
    {
        public DealUiData(Sprite icon)
        {
            Icon = icon;
        }
        public Sprite Icon { get; set; }
        public List<DealButtonData> Buttons { get; set; } = new();
    }
}