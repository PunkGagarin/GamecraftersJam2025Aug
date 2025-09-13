using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BallDto
    {
        public int Id { get; set; }
        public Sprite Sprite { get; set; }
        public BallType Type { get; set; }
        public int Grade { get; set; }

        public string Description { get; set; }

        public BallDto(int id, Sprite sprite, BallType type, int grade, string description)
        {
            Id = id;
            Sprite = sprite;
            Type = type;
            Grade = grade;
            Description = description;
        }

        public BallDto(PlayerBallModel ball)
        {
            Id = ball.BallId;
            Sprite = ball.Sprite;
            Type = ball.Type;
            Grade = ball.Grade;
            Description = ball.Description;
        }

    }
}