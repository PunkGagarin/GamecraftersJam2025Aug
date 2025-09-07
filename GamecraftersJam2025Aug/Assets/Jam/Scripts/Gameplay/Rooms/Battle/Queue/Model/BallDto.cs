using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BallDto
    {
        public int Id { get; set; }
        public Sprite Sprite { get; set; }
        public string Description { get; set; }

        public BallDto(int id, Sprite sprite, string description)
        {
            Id = id;
            Sprite = sprite;
            Description = description;
        }
    }
}