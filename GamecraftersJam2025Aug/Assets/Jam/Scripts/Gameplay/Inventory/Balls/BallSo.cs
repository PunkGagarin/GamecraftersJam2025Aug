using UnityEngine;

namespace Jam.Scripts.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Gameplay/Ball/PlayerBalls", fileName = "BallSo", order = 0)]
    public class BallSo : ScriptableObject
    {
        [field: SerializeField]
        public int Damage { get; private set; }
    }
}