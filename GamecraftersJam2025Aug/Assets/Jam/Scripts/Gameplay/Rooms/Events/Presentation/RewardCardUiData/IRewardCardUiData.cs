using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public interface IRewardCardUiData
    {
        public Sprite Icon { get; set; }
        public string Desc { get; set; }
    }
}