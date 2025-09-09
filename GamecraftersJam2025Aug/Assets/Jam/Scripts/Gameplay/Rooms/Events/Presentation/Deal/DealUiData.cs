using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using UnityEngine;

public class DealUiData
{
    public Sprite Icon { get; set; }
    public List<DealButtonData> Buttons { get; set; } = new();
}