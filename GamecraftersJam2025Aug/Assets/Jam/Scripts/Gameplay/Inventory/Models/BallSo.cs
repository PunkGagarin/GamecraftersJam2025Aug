using Jam.Scripts.Gameplay.Configs;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    [CreateAssetMenu(menuName = "Gameplay/Ball/PlayerBalls", fileName = "BallSo", order = 0)]
    public class BallSo : ScriptableObject
    {
        [field: SerializeField]
        public int Damage { get; private set; }

        [field: SerializeField]
        public BallType BallType { get; private set; }

        [field: SerializeField]
        public TargetType TargetType { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; set; }
    }

}