using Jam.Scripts.Gameplay.Configs;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BallRewardDto
    {
        public Sprite Sprite { get; set; }
        public string Description { get; set; }
        public BallType Type { get; set; }

        public BallRewardDto(BallType type, Sprite sprite, string description)
        {
            Sprite = sprite;
            Description = description;
            Type = type;
        }
    }
}