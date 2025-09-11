using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BallRewardDto
    {
        public Sprite Sprite { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; }
        public BallType Type { get; set; }

        public BallRewardDto(Sprite sprite, int grade, string description, BallType type)
        {
            Sprite = sprite;
            Grade = grade;
            Description = description;
            Type = type;
        }
    }
}