using Jam.Scripts.Gameplay.Artifacts;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class ArtifactRewardCardUiData : IRewardCardUiData
    {
        public ArtifactRewardCardUiData(Sprite icon, string desc, ArtifactType type)
        {
            Type = type;
            Icon = icon;
            Desc = desc;
        }
        public ArtifactType Type { get; set; }
        public Sprite Icon { get; set; }
        public string Desc { get; set; }
    }
}