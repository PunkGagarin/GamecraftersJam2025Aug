using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    // [CreateAssetMenu(menuName = "Gameplay/ShellGameConfig", fileName = "ShellGameConfig", order = 0)]
    public class ShellGameConfig : ScriptableObject
    {
        [field: SerializeField]
        public int TryCount { get; private set; }
        
        [field: SerializeField]
        public CupView CupViewPrefab { get; private set; }

        [field: SerializeField]
        public BallView GreenBallViewPrefab { get; private set; }

        [field: SerializeField]
        public BallView RedBallViewPrefab { get; private set; }

        [field: SerializeField]
        public int CupCount { get; private set; }

        [field: SerializeField]
        public int GreenBallCount { get; private set; }

        [field: SerializeField]
        public int RedBallCount { get; private set; }

        [field: SerializeField]
        public int MinShuffleCount { get; private set; }

        [field: SerializeField]
        public int MaxShuffleCount { get; private set; }

        [field: SerializeField]
        public float NextPairPickupSpeed { get; private set; }

        [field: SerializeField]
        public float CupShuffleSpeed { get; private set; }

    }
}