using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    public class PlayerBallModel
    {
        public int BallId { get; set; }
        public TargetType TargetType { get; set; }
        public int Damage { get; private set; }
        public Sprite Sprite { get; set; }
        public List<EffectInstance> Effects { get; set; }

        public PlayerBallModel(int ballId, int damage, TargetType targetType, Sprite sprite, List<EffectInstance> effects)
        {
            Damage = damage;
            BallId = ballId;
            TargetType = targetType;
            Sprite = sprite;
            Effects = effects;
        }

        public string Description => $" Это описание шара: TargetType: {TargetType}, {Damage} damage";
        public PlayerBallModel Clone() => new(BallId, Damage, TargetType, Sprite, Effects);
    }
}