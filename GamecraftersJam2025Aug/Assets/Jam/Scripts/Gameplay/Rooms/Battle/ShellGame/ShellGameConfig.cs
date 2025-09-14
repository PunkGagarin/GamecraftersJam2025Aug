using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Rooms.Battle.ShellGame;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    // [CreateAssetMenu(menuName = "Gameplay/ShellGameConfig", fileName = "ShellGameConfig", order = 0)]
    public class ShellGameConfig : ScriptableObject
    {
        [field: SerializeField]
        public List<CustomKeyValue<int, ShellGameCupAndBallInfo>> CupAndBallInfo { get; private set; }


        [field: SerializeField]
        public int MaxCupCount { get; private set; } = 6;
        
        [field: SerializeField]
        public CupView CupViewPrefab { get; private set; }

        [field: SerializeField]
        public BoardBallView GreenBallViewPrefab { get; private set; }

        [field: SerializeField]
        public BoardBallView RedBallViewPrefab { get; private set; }

        [field: SerializeField]
        public int MinShuffleCount { get; private set; }

        [field: SerializeField]
        public int MaxShuffleCount { get; private set; }

        [field: SerializeField]
        public float NextPairPickupSpeed { get; private set; }

        [field: SerializeField]
        public float CupShuffleSpeed { get; private set; }

        public ShellGameCupAndBallInfo GetInfoFor(int cupCount) =>
            CupAndBallInfo
                .FirstOrDefault(cupAndBallInfo => cupAndBallInfo.Key == cupCount)
                ?.Value;
    }

    [Serializable]
    public class ShellGameCupAndBallInfo
    {
        [field: SerializeField]
        public int CupCount { get; private set; }

        [field: SerializeField]
        public int RedBallCount { get; private set; }
    }
}