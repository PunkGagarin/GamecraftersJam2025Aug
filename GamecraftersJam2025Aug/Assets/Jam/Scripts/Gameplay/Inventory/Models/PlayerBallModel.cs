using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class PlayerBallModel
    {
        public int BallId { get; set; }
        public Sprite Sprite { get; set; }
        public List<EffectInstance> Effects { get; set; }

        public PlayerBallModel(int ballId, Sprite sprite, List<EffectInstance> effects)
        {
            BallId = ballId;
            Sprite = sprite;
            Effects = effects;
        }

        public string Description => $" Это описание шара: ";
        public PlayerBallModel Clone() => new(BallId, Sprite, Effects);
    }
}