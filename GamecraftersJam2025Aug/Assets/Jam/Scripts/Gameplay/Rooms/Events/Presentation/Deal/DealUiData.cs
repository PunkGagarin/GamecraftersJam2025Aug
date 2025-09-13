using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class DealUiData
    {
        public DealUiData(Sprite icon, List<string> clownMonologueStrings)
        {
            Icon = icon;
            ClownMonologueStrings = clownMonologueStrings;
        }
        public Sprite Icon { get; set; }
        public List<DealButtonData> Buttons { get; set; } = new();
        
        public List<string> ClownMonologueStrings { get; set; }
    }
}