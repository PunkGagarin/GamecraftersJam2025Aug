using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class PlayerBallModel
    {
        public int BallId { get; set; }
        public Sprite Sprite { get; set; }
        public List<EffectInstance> Effects { get; set; }
        public int Grade { get; set; }
        public BallType Type { get; set; }
        public string Description { get; set; }

        public PlayerBallModel(int ballId, BallType type, int grade, Sprite sprite, List<EffectInstance> effects)
        {
            BallId = ballId;
            Type = type;
            Grade = grade;
            Sprite = sprite;
            Effects = effects;
        }


        public PlayerBallModel(int ballId, BallType type, int grade,
            Sprite sprite, List<EffectInstance> effects, string description)
            : this(ballId, type, grade, sprite, effects)
        {
            Description = description;
        }

        public PlayerBallModel Clone() => new(BallId, Type, Grade, Sprite, Effects, Description);
    }
}